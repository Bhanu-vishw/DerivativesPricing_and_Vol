using System;
using MathNet.Numerics.Distributions;

class MonteCarloSimulator
{
    static Random rand = new Random();

    static double NormalRandom()
    {
        double u1 = rand.NextDouble();
        double u2 = rand.NextDouble();
        double z;

        if (u1 > 0 && u2 > 0)
        {
            z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
        }
        else
        {
            z = 0.0;
        }

        return z;
    }

    public static double InvNormalCdf(double p)
    {
        return Normal.InvCDF(0, 1, p);
    }

    public static (double price, double delta, double gamma, double vega, double rho, double theta, double[] payoffs) PriceOption(
        double S0, double K, double r, double sigma, double T, string optionType,
        int sims, bool antithetic)
    {
        double[] payoffs = new double[antithetic ? sims / 2 : sims];
        int n = payoffs.Length;
        double epsilon = 0.01;
        double sumDelta = 0.0, sumGamma = 0.0, sumVega = 0.0, sumRho = 0.0, sumTheta = 0.0;

        for (int i = 0; i < n; i++)
        {
            double z = NormalRandom();

            double ST = S0 * Math.Exp((r - 0.5 * sigma * sigma) * T + sigma * Math.Sqrt(T) * z);
            double payoff;

            if (optionType == "call") payoff = Math.Max(ST - K, 0);
            else payoff = Math.Max(K - ST, 0);

            // Delta & Gamma
            double ST_up = (S0 + epsilon) * Math.Exp((r - 0.5 * sigma * sigma) * T + sigma * Math.Sqrt(T) * z);
            double ST_down = (S0 - epsilon) * Math.Exp((r - 0.5 * sigma * sigma) * T + sigma * Math.Sqrt(T) * z);
            double payoff_up, payoff_down;
            if (optionType == "call")
            {
                payoff_up = Math.Max(ST_up - K, 0);
                payoff_down = Math.Max(ST_down - K, 0);
            }
            else
            {
                payoff_up = Math.Max(K - ST_up, 0);
                payoff_down = Math.Max(K - ST_down, 0);
            }
            double delta = (payoff_up - payoff_down) / (2 * epsilon);
            double gamma = (payoff_up - 2 * payoff + payoff_down) / (epsilon * epsilon);

            // Vega
            double ST_vega = S0 * Math.Exp((r - 0.5 * (sigma + epsilon) * (sigma + epsilon)) * T + (sigma + epsilon) * Math.Sqrt(T) * z);
            double payoff_vega;
            if (optionType == "call") payoff_vega = Math.Max(ST_vega - K, 0);
            else payoff_vega = Math.Max(K - ST_vega, 0);
            double vega = (payoff_vega - payoff) / epsilon;

            // Rho
            double ST_rho = S0 * Math.Exp((r + epsilon - 0.5 * sigma * sigma) * T + sigma * Math.Sqrt(T) * z);
            double payoff_rho;
            if (optionType == "call") payoff_rho = Math.Max(ST_rho - K, 0);
            else payoff_rho = Math.Max(K - ST_rho, 0);
            double rho = (payoff_rho - payoff) / epsilon;

            // Theta
            double T_theta = T - epsilon;
            double ST_theta = S0 * Math.Exp((r - 0.5 * sigma * sigma) * T_theta + sigma * Math.Sqrt(T_theta) * z);
            double payoff_theta;
            if (optionType == "call") payoff_theta = Math.Max(ST_theta - K, 0);
            else payoff_theta = Math.Max(K - ST_theta, 0);
            double theta = (payoff_theta - payoff) / epsilon;

            if (antithetic)
            {
                double ST2 = S0 * Math.Exp((r - 0.5 * sigma * sigma) * T - sigma * Math.Sqrt(T) * z);
                double payoff2;
                if (optionType == "call") payoff2 = Math.Max(ST2 - K, 0);
                else payoff2 = Math.Max(K - ST2, 0);
                payoff = (payoff + payoff2) / 2.0;
            }

            payoffs[i] = payoff;
            sumDelta += delta;
            sumGamma += gamma;
            sumVega += vega;
            sumRho += rho;
            sumTheta += theta;
        }

        double avgPayoff = 0.0;
        foreach (double p in payoffs) avgPayoff += p;
        avgPayoff /= n;

        double discount = Math.Exp(-r * T);
        return (
            discount * avgPayoff,
            sumDelta / n,
            sumGamma / n,
            sumVega / n,
            sumRho / n,
            sumTheta / n,
            payoffs
        );
    }

    public static double StandardError(double[] payoffs, double r, double T)
    {
        double avg = 0.0;
        foreach (double p in payoffs) avg += p;
        avg /= payoffs.Length;

        double variance = 0.0;
        foreach (double p in payoffs) variance += (p - avg) * (p - avg);
        variance /= (payoffs.Length - 1);

        return Math.Exp(-r * T) * Math.Sqrt(variance / payoffs.Length);
    }

    public static double VanDerCorput(int n, int baseNum)
    {
        double vdc = 0.0;
        double denom = 1.0;

        while (n > 0)
        {
            denom *= baseNum;
            vdc += (n % baseNum) / denom;
            n /= baseNum;
        }

        return vdc;
    }

    public static (double price, double delta, double gamma, double vega, double rho, double theta) PriceOptionVDC(
        double S0, double K, double r, double sigma, double T,
        string optionType, int sims, int baseNum)
    {
        double sumPayoff = 0.0, sumDelta = 0.0, sumGamma = 0.0, sumVega = 0.0, sumRho = 0.0, sumTheta = 0.0;
        double epsilon = 0.01;

        for (int i = 1; i <= sims; i++)
        {
            double z = InvNormalCdf(VanDerCorput(i, baseNum));
            double ST = S0 * Math.Exp((r - 0.5 * sigma * sigma) * T + sigma * Math.Sqrt(T) * z);
            double payoff;
            if (optionType == "call") payoff = Math.Max(ST - K, 0);
            else payoff = Math.Max(K - ST, 0);

            double ST_up = (S0 + epsilon) * Math.Exp((r - 0.5 * sigma * sigma) * T + sigma * Math.Sqrt(T) * z);
            double ST_down = (S0 - epsilon) * Math.Exp((r - 0.5 * sigma * sigma) * T + sigma * Math.Sqrt(T) * z);
            double payoff_up, payoff_down;
            if (optionType == "call")
            {
                payoff_up = Math.Max(ST_up - K, 0);
                payoff_down = Math.Max(ST_down - K, 0);
            }
            else
            {
                payoff_up = Math.Max(K - ST_up, 0);
                payoff_down = Math.Max(K - ST_down, 0);
            }

            double delta = (payoff_up - payoff_down) / (2 * epsilon);
            double gamma = (payoff_up - 2 * payoff + payoff_down) / (epsilon * epsilon);

            double ST_vega = S0 * Math.Exp((r - 0.5 * (sigma + epsilon) * (sigma + epsilon)) * T + (sigma + epsilon) * Math.Sqrt(T) * z);
            double payoff_vega;
            if (optionType == "call") payoff_vega = Math.Max(ST_vega - K, 0);
            else payoff_vega = Math.Max(K - ST_vega, 0);
            double vega = (payoff_vega - payoff) / epsilon;

            double ST_rho = S0 * Math.Exp((r + epsilon - 0.5 * sigma * sigma) * T + sigma * Math.Sqrt(T) * z);
            double payoff_rho;
            if (optionType == "call") payoff_rho = Math.Max(ST_rho - K, 0);
            else payoff_rho = Math.Max(K - ST_rho, 0);
            double rho = (payoff_rho - payoff) / epsilon;

            double T_theta = T - epsilon;
            double ST_theta = S0 * Math.Exp((r - 0.5 * sigma * sigma) * T_theta + sigma * Math.Sqrt(T_theta) * z);
            double payoff_theta;
            if (optionType == "call") payoff_theta = Math.Max(ST_theta - K, 0);
            else payoff_theta = Math.Max(K - ST_theta, 0);
            double theta = (payoff_theta - payoff) / epsilon;

            sumPayoff += payoff;
            sumDelta += delta;
            sumGamma += gamma;
            sumVega += vega;
            sumRho += rho;
            sumTheta += theta;
        }

        double discount = Math.Exp(-r * T);
        return (
            discount * (sumPayoff / sims),
            sumDelta / sims,
            sumGamma / sims,
            sumVega / sims,
            sumRho / sims,
            sumTheta / sims
        );
    }

    static void Main()
    {
        Console.Write("Initial Stock Price (S0): ");
        double S0 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Strike Price (K): ");
        double K = Convert.ToDouble(Console.ReadLine());

        Console.Write("Time to Maturity (T in years): ");
        double T = Convert.ToDouble(Console.ReadLine());

        Console.Write("Risk-free Rate (r): ");
        double r = Convert.ToDouble(Console.ReadLine());

        Console.Write("Volatility (sigma): ");
        double sigma = Convert.ToDouble(Console.ReadLine());

        Console.Write("Number of simulations: ");
        int simulations = Convert.ToInt32(Console.ReadLine());

        Console.Write("Use antithetic sampling? (true/false): ");
        bool useAntithetic = Convert.ToBoolean(Console.ReadLine());

        Console.Write("Enter base for Van der Corput: ");
        int baseVDC = Convert.ToInt32(Console.ReadLine());

        Console.Write("Option type (call/put): ");
        string type = Console.ReadLine()?.ToLower() ?? "call";

        var (price, delta, gamma, vega, rho, theta, payoffs) = PriceOption(S0, K, r, sigma, T, type, simulations, useAntithetic);
        double stderr = StandardError(payoffs, r, T);

        Console.WriteLine($"\nOption Price (antithetic = {useAntithetic}): {price:F4}");
        Console.WriteLine($"Delta  (antithetic = {useAntithetic}): {delta:F4}");
        Console.WriteLine($"Gamma  (antithetic = {useAntithetic}): {gamma:F4}");
        Console.WriteLine($"Vega   (antithetic = {useAntithetic}): {vega:F4}");
        Console.WriteLine($"Rho    (antithetic = {useAntithetic}): {rho:F4}");
        Console.WriteLine($"Theta  (antithetic = {useAntithetic}): {theta:F4}");
        Console.WriteLine($"Standard Error: {stderr:F4}");

        Console.WriteLine("\nVan der Corput Pricing:");
        var (priceVDC, deltaVDC, gammaVDC, vegaVDC, rhoVDC, thetaVDC) = PriceOptionVDC(S0, K, r, sigma, T, type, simulations, baseVDC);
        Console.WriteLine($"Price  (Van der Corput): {priceVDC:F4}");
        Console.WriteLine($"Delta  (Van der Corput): {deltaVDC:F4}");
        Console.WriteLine($"Gamma  (Van der Corput): {gammaVDC:F4}");
        Console.WriteLine($"Vega   (Van der Corput): {vegaVDC:F4}");
        Console.WriteLine($"Rho    (Van der Corput): {rhoVDC:F4}");
        Console.WriteLine($"Theta  (Van der Corput): {thetaVDC:F4}");
    }
}
