using System;

class Program
{
    // Shared random generator
    static Random r = new Random();

    // Using Box-Muller method
    static double GenerateNormal()
    {
        double u1 = r.NextDouble();
        double u2 = r.NextDouble();
        return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
    }

    // Simulate final stock price and return option payoff
    static double SimulatePayoff(double S0, double K, double r, double sigma, double T, int steps, string type)
    {
        double dt = T / steps;
        double S = S0;

        // Generate asset path using geometric Brownian motion
        for (int i = 0; i < steps; i++)
        {
            double z = GenerateNormal();
            S *= Math.Exp((r - 0.5 * sigma * sigma) * dt + sigma * Math.Sqrt(dt) * z);
        }

        // Calculate payoff at maturity
        if (type == "call")
            return Math.Max(S - K, 0);
        else
            return Math.Max(K - S, 0);
    }

    // Run simulations and return average discounted price
    static double GetOptionPrice(double S0, double K, double r, double sigma, double T, int steps, int simulations, string type)
    {
        double sum = 0;

        // Run simulation loop
        for (int i = 0; i < simulations; i++)
        {
            double payoff = SimulatePayoff(S0, K, r, sigma, T, steps, type);
            sum += payoff;
        }

        double avg = sum / simulations;
        return Math.Exp(-r * T) * avg; // discounting to present value
    }

    // Greeks using finite difference
    static void CalculateGreeks(double S0, double K, double r, double sigma, double T, int steps, int simulations, string type)
    {
        double h = 0.01; // small change

        double basePrice = GetOptionPrice(S0, K, r, sigma, T, steps, simulations, type);

        // Delta
        double priceUp = GetOptionPrice(S0 + h, K, r, sigma, T, steps, simulations, type);
        double priceDown = GetOptionPrice(S0 - h, K, r, sigma, T, steps, simulations, type);
        double delta = (priceUp - priceDown) / (2 * h);

        // Gamma
        double gamma = (priceUp - 2 * basePrice + priceDown) / (h * h);

        // Vega
        double vega = (GetOptionPrice(S0, K, r, sigma + h, T, steps, simulations, type) - GetOptionPrice(S0, K, r, sigma - h, T, steps, simulations, type)) / (2 * h);

        // Theta
        double theta = (GetOptionPrice(S0, K, r, sigma, T - h, steps, simulations, type) - basePrice) / (-h);

        // Rho
        double rho = (GetOptionPrice(S0, K, r + h, sigma, T, steps, simulations, type) - GetOptionPrice(S0, K, r - h, sigma, T, steps, simulations, type)) / (2 * h);

        // Display all Greeks
        Console.WriteLine("\n--- Greeks ---");
        Console.WriteLine($"Delta: {delta:F4}");
        Console.WriteLine($"Gamma: {gamma:F4}");
        Console.WriteLine($"Vega:  {vega:F4}");
        Console.WriteLine($"Theta: {theta:F4}");
        Console.WriteLine($"Rho:   {rho:F4}");
    }

    static void RunMonteCarlo()
    {
        Console.WriteLine("=== Monte Carlo Option Pricing ===");

        // Get user inputs
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

        Console.Write("Steps per simulation: ");
        int steps = Convert.ToInt32(Console.ReadLine());

        Console.Write("Number of simulations: ");
        int simulations = Convert.ToInt32(Console.ReadLine());

        Console.Write("Option type (call/put): ");
        string type = Console.ReadLine()?.ToLower() ?? "call";

        // Calculate option price
        double price = GetOptionPrice(S0, K, r, sigma, T, steps, simulations, type);
        Console.WriteLine($"\nEstimated Option Price: {price:F4}");

        // Calculate Greeks
        CalculateGreeks(S0, K, r, sigma, T, steps, simulations, type);
    }

    static void Main()
    {
        RunMonteCarlo();
    }
}
