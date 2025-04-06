using System;

class Program
{
    // Sum Twelve Method
    static double SumTwelve()
    {
        Random r = new Random();
        double sum = 0;

        for (int i = 0; i < 12; i++)
        {
            sum += r.NextDouble();
        }

        return sum - 6;
    }

    // BoxMuller Method
    static (double, double) BoxMuller()
    {
        Random r = new Random();
        double x1 = r.NextDouble();
        double x2 = r.NextDouble();

        // Applying formula
        double factor = Math.Sqrt(-2.0 * Math.Log(x1));
        double theta = 2.0 * Math.PI * x2;

        double z1 = factor * Math.Cos(theta);
        double z2 = factor * Math.Sin(theta);

        return (z1, z2);
    }

    // Polar Rejection Method
    static (double, double) PolarRejection()
    {
        Random r = new Random();
        double x1, x2, w, c, z1, z2;

        do
        {
            x1 = 2.0 * r.NextDouble() - 1.0; // for range [-1, 1]
            x2 = 2.0 * r.NextDouble() - 1.0; // for range [-1, 1]

            w = x1 * x1 + x2 * x2;

        } while (w >= 1.0);

        c = Math.Sqrt(-2.0 * Math.Log(w) / w);
        z1 = c * x1;
        z2 = c * x2;

        return (z1, z2);
    }

    // Correlated Gaussian Normal Randoms
    static (double, double) GenerateCorrelatedNormals(double rho)
    {
        // Generate two independent standard normals
        double z1 = PolarRejection().Item1; // or SumTwelve(), PolarRejection().Item1

        double z2_indep = PolarRejection().Item1;

        // Correlated z2
        double z2_corr = rho * z1 + Math.Sqrt(1 - rho * rho) * z2_indep;

        return (z1, z2_corr);
    }

    static void Main()
    {
        Console.WriteLine("Choose method: 1=SumTwelve, 2=Box-Muller, 3=Polar Rejection");
        string method = Console.ReadLine();

        Console.Write("Enter correlation (between -1 and 1): ");
        double rho = double.Parse(Console.ReadLine());

        double z1 = 0, z2 = 0; // Default initialization of z1 and z2

        switch (method)
        {
            case "1":
                z1 = SumTwelve();
                z2 = GenerateCorrelatedNormals(rho).Item2;
                break;
            case "2":
                (z1, _) = BoxMuller();
                (_, z2) = GenerateCorrelatedNormals(rho);
                break;
            case "3":
                (z1, _) = PolarRejection();
                (_, z2) = GenerateCorrelatedNormals(rho);
                break;
            default:
                Console.WriteLine("Invalid selection.");
                return;
        }

        Console.WriteLine($"Correlated Normal Samples: z1 = {z1}, z2 = {z2}");
    }
}
