using System.Diagnostics;

namespace Pixel_Creator
{
    public partial class main : Form
    {
        int[,] pixel_matrix = new int[7, 5];
        string default_result_text = "LCDOUT $fe, $40, 0, 0, 0, 0, 0, 0, 0";

        public main()
        {
            InitializeComponent();
            OnStart();
        }

        void OnStart()
        {
            // Initially filling pixel matrix with 0's
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    pixel_matrix[i, j] = 0;
                }
            }
            // And setting result to default
            result.Text = default_result_text;
        }

        void calculate()
        // When a button is clicked this function calculates and prints the new outcome
        {
            string result_text = "LCDOUT $fe, $40";
            for (int i = 0; i < 7; i++)
            {
                result_text += ", ";
                int temp_result = 0;
                int powers_of_two = 16;
                for (int j = 0; j < 5; j++)
                {
                    temp_result += powers_of_two * pixel_matrix[i, j]; // Converting binary to decimal  
                    powers_of_two /= 2;
                }
                result_text += temp_result.ToString();
            }
            result.Text = result_text;
        }

        private void button_Click(object sender, EventArgs e)
        {
            // Determining which button is clicked
            Button button = sender as Button;
            char[] btn = button.Name.ToCharArray();
            // Buttons names are bRxC (R: Row, C: Column)
            int i = Int16.Parse(btn[1].ToString());
            int j = Int16.Parse(btn[3].ToString());

            // Changing buttons colors
            if (pixel_matrix[i, j] == 0)
            {
                button.BackColor = Color.Black;
                pixel_matrix[i, j] = 1;
            }
            else
            {
                button.BackColor = Color.White;
                pixel_matrix[i, j] = 0;
            }
            calculate();
        }

        private void github_Click(object sender, EventArgs e)
        // Opens a new tab to my Github page
        {
            Process.Start(new ProcessStartInfo { FileName = @"https://github.com/Jupkobe", UseShellExecute = true });
        }

        private void copy_Click(object sender, EventArgs e)
        // Copies the result to clipboard
        {
            Clipboard.SetText(result.Text);
        }

        private void clear_Click(object sender, EventArgs e)
        // Clears the pixel matrix and changes all the buttons color to white
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    // Buttons names are bRxC (R: Row, C: Column)
                    string button_name = "b" + i.ToString() + "x" + j.ToString();
                    Button button = (Button)this.Controls[button_name];
                    button.BackColor = Color.White;
                    pixel_matrix[i, j] = 0;
                }
            }
            result.Text = default_result_text;
        }

        private void result_KeyPress(object sender, KeyPressEventArgs e)
        // User can type their pixel model and this function reverse engineers and colors the buttons
        {
            if (e.KeyChar == (char)13) // When pressed "Enter"
            {
                e.Handled = true; // Windows alert sound disabled
                string[] inp = result.Text.ToString().Split(","); // Input is splitted with ","
                for (int i = 0; i < 7; i++)
                {
                    int number = 0;
                    int powers_of_two = 16;
                    try // if user types other than int
                    {
                        number = Int16.Parse(inp[i + 2]);
                    }
                    catch
                    {
                        continue;
                    }
                    for (int j = 0; j < 5; j++)
                    {
                        if (number >= powers_of_two)
                        {
                            number -= powers_of_two; // Converting decimal to binary
                            pixel_matrix[i, j] = 1;
                            string button_name = "b" + i.ToString() + "x" + j.ToString();
                            Button button = (Button)this.Controls[button_name];
                            button.BackColor = Color.Black;
                        }
                        else
                        {
                            pixel_matrix[i, j] = 0;
                            string button_name = "b" + i.ToString() + "x" + j.ToString();
                            Button button = (Button)this.Controls[button_name];
                            button.BackColor = Color.White;
                        }
                        powers_of_two /= 2;
                        calculate();
                    }
                }
            }
        }
    }
}