using Mwm.BeerFactoryV2.Service.Components;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Pid {
    /// <summary>
    /// A (P)roportional, (I)ntegral, (D)erivative Controller
    /// </summary>
    /// <remarks>
    /// The controller should be able to control any process with a
    /// measureable value, a known ideal value and an input to the
    /// process that will affect the measured value.
    /// </remarks>
    /// <see cref="https://en.wikipedia.org/wiki/PID_controller"/>
    public sealed class PidController {
        private Ssr ssr;
        private Thermometer thermometer;

        private double processVariable = 0;
        private DateTime lastRun;
        private bool isRunning = false;

        private int dutyCycleInMillis = 2000;

        public PidController(Ssr ssr, Thermometer thermometer) {
            this.ssr = ssr;
            this.thermometer = thermometer;
        }

        public PidController(Ssr ssr, Thermometer thermometer, double setPoint) {
            this.ssr = ssr;
            this.thermometer = thermometer;
            this.SetPoint = setPoint;
        }

        public PidController(Ssr ssr, Thermometer thermometer, double gainProportional, double gainIntegral, double gainDerivative, double outputMin, double outputMax, double setPoint) {
            if (OutputMax < OutputMin)
                throw new FormatException("OutputMax is less than OutputMin");
            this.ssr = ssr;
            this.thermometer = thermometer;
            this.GainDerivative = gainDerivative;
            this.GainIntegral = gainIntegral;
            this.GainProportional = gainProportional;
            this.OutputMax = outputMax;
            this.OutputMin = outputMin;
            this.SetPoint = setPoint;
        }

        public void Start() {
            isRunning = true;
 
            // Call new thread to run
            Task.Run(() => Run());
        }

        public void Stop() {
            isRunning = false;
        }

        private void Run() {
            while (isRunning) {
                Process();
                Thread.Sleep(dutyCycleInMillis);
            }
        }


        /// <summary>
        /// The controller output
        /// </summary>
        /// <param name="timeSinceLastUpdate">timespan of the elapsed time
        /// since the previous time that ControlVariable was called</param>
        /// <returns>Value of the variable that needs to be controlled</returns>
        public void Process() {
            
            ProcessVariable = (double)thermometer.Temperature;

            if (ProcessVariable != 0) {
                var currentTime = DateTime.Now;
                if (lastRun == null)
                    lastRun = currentTime;


                var secondsSinceLastUpdate = (currentTime - lastRun).Seconds;

                double error = SetPoint - ProcessVariable;

                // integral term calculation
                IntegralTerm += (GainIntegral * error * secondsSinceLastUpdate);
                IntegralTerm = Clamp(IntegralTerm);

                // derivative term calculation
                double dInput = processVariable - ProcessVariableLast;
                double derivativeTerm = GainDerivative * (dInput / secondsSinceLastUpdate);

                // proportional term calcullation
                double proportionalTerm = GainProportional * error;

                double output = proportionalTerm + IntegralTerm - derivativeTerm;

                output = Clamp(output);

                lastRun = currentTime;

                Debug.WriteLine($"Temperature: {ProcessVariable}  SSR: {output}");

                ssr.Percentage = (int)output;
            }
        }



        /// <summary>
        /// The derivative term is proportional to the rate of
        /// change of the error
        /// </summary>
        public double GainDerivative { get; set; } = 1;

        /// <summary>
        /// The integral term is proportional to both the magnitude
        /// of the error and the duration of the error
        /// </summary>
        public double GainIntegral { get; set; } = 1;

        /// <summary>
        /// The proportional term produces an output value that
        /// is proportional to the current error value
        /// </summary>
        /// <remarks>
        /// Tuning theory and industrial practice indicate that the
        /// proportional term should contribute the bulk of the output change.
        /// </remarks>
        public double GainProportional { get; set; } = 1;

        /// <summary>
        /// The max output value the control device can accept.
        /// </summary>
        public double OutputMax { get; private set; } = 100;

        /// <summary>
        /// The minimum ouput value the control device can accept.
        /// </summary>
        public double OutputMin { get; private set; } = 0;

        /// <summary>
        /// Adjustment made by considering the accumulated error over time
        /// </summary>
        /// <remarks>
        /// An alternative formulation of the integral action, is the
        /// proportional-summation-difference used in discrete-time systems
        /// </remarks>
        public double IntegralTerm { get; private set; } = 0;


        /// <summary>
        /// The current value
        /// </summary>
        public double ProcessVariable {
            get { return processVariable; }
            set {
                ProcessVariableLast = processVariable;
                processVariable = value;
            }
        }

        /// <summary>
        /// The last reported value (used to calculate the rate of change)
        /// </summary>
        public double ProcessVariableLast { get; private set; } = 0;

        /// <summary>
        /// The desired value
        /// </summary>
        public double SetPoint { get; set; } = 0;

        /// <summary>
        /// Limit a variable to the set OutputMax and OutputMin properties
        /// </summary>
        /// <returns>
        /// A value that is between the OutputMax and OutputMin properties
        /// </returns>
        /// <remarks>
        /// Inspiration from http://stackoverflow.com/questions/3176602/how-to-force-a-number-to-be-in-a-range-in-c
        /// </remarks>
        private double Clamp(double variableToClamp) {
            if (variableToClamp <= OutputMin) { return OutputMin; }
            if (variableToClamp >= OutputMax) { return OutputMax; }
            return variableToClamp;
        }
    }
}
