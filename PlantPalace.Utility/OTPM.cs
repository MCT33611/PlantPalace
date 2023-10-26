namespace PlantPalace.Models
{
    public class OTPM 
    {
        public static int OTP { get; private set; } // Make the OTP property public
        public static string? Email { get; set; } // User email to sent otp

        // Method to generate and set a new OTP
        public static void GenerateOTP()
        {
            Random random = new Random();
            OTP = random.Next(100000, 999990);
        }
    }
}
