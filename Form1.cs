// Programmer: Alysia Maxwell
// Project: Assignment 3 - Balloon order form 
// Due Date: 11/07/2018
// Description: Assignment #3

using System;
using System.Windows.Forms;
using System.IO;

namespace Maxwell_3
{
    public partial class BonniesBalloons : Form
    {
        // Declare Class Level Constants To Prevent From Declaring Local Variables Inside Each Event 

        private const decimal HOME_DELIVERY = 7.50m;
        private const decimal SINGLE_BALLOON = 9.95m;
        private const decimal HALF_DOZEN_BALLOON = 35.95m;
        private const decimal DOZEN_BALLOON = 65.95m;
        private const decimal EXTRAS = 9.50m;
        private const decimal PERSONALIZED_MESSAGE = 2.50m;
        private const decimal TAX_RATE = 0.07m;

        // Class level variables used to calculate totals 

        private decimal subtotal;
        private decimal orderTotal;
        private decimal salesTaxAmount;
        private int extraCount = 0;
       
        // Declare variables used in form 

        string deliveryType;
        string bundleSize;
        string occasion;
        string extras;
        string extraList;
        
        public BonniesBalloons()
        {
            InitializeComponent();
        }

        // Immediately executes when form loads
         
        private void BonniesBalloons_Load(object sender, EventArgs e)
        {
            // When the form loads the current date will display in the date of sale text box

               deliveryDateMaskedTextBox.Text = DateTime.Now.ToString("MM/dd/yyyy");

            // Display constant prices to labels on the form when it loads

               deliveryChargeLabel.Text = HOME_DELIVERY.ToString("C");
               singleLabel.Text = SINGLE_BALLOON.ToString("C");
               halfDozenLabel.Text = HALF_DOZEN_BALLOON.ToString("C");
               dozenLabel.Text = DOZEN_BALLOON.ToString("C");
               personalizedMessagePriceLabel.Text = PERSONALIZED_MESSAGE.ToString("C");
               extraPriceLabel.Text = EXTRAS.ToString("C");
               subtotalSummaryLabel.Text = subtotal.ToString("C");
               totalTaxLabel.Text = salesTaxAmount.ToString("C");
               totalLabel.Text = orderTotal.ToString("C");            

            // Call to method PopulateBoxes to fill out combo box

            PopulateBoxes();

            // Set default selection to birthday for combo box 

            specialOccasionComboBox.SelectedItem = "Birthday";

            // Call to method to update order totals 

            UpdateTotals();
           
        } 

        // Custom method to read data from the file to fill the combo and list box  
           
        private void PopulateBoxes()
        {

                // Try catch block to read data from file without error 
            
            try
            {
               
                // Declare streamReader object

                StreamReader inputFile;

                // Open file 

                inputFile = File.OpenText("Occasion.txt");

                // Verify all data is read from the file 

                while (!inputFile.EndOfStream) 
                {
                    // Get occasion name

                    occasion = inputFile.ReadLine(); 

                    // Add occasion to the special occasion combo box 

                       specialOccasionComboBox.Items.Add(occasion);
                }

                // Close the File

                inputFile.Close(); 
                 
            } 
            catch (Exception ex)
            {
                // Display error message if there is a problem reading from the file 

                   MessageBox.Show(ex.Message);

            }

            // Try catch block to read data from file without error and input in list box

            try
            {
               
                // Declare streamReader object

                StreamReader inputFile;

                // Open file 

                inputFile = File.OpenText("Extras.txt");

                // Verify all data is read from the file 

                while (!inputFile.EndOfStream)
                {
                    // Get name of extra item 

                    extras = inputFile.ReadLine();

                    // Add extra item to the extra list box 

                    extrasListBox.Items.Add(extras);

                }

                // Close the File

                inputFile.Close(); 

            }
            catch (Exception ex)
            {
                // Display error message if there is a problem reading from the file 

                MessageBox.Show(ex.Message);

            }
           
        }

        // Checked change event for personal message added to order

        private void personalizedMessageCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // Call to method to update subtotal if message is added to the order

            UpdateTotals();

            // If Checked personal message enables and when unchecked cleared and disabled 

            if ( personalizedMessageCheckBox.Checked)
            {
                // Enable text box 

                personalMessageTextBox.Enabled = true;
                         
            }
            else
            {
                 // Disable and clear personal message text box

                personalMessageTextBox.Enabled = false;
                personalMessageTextBox.Text = "";

            }
        }

        // Custom method to clear the form and set it back to default appearance

        private void ResetForm()
        {
            // focus sent back to the first data entry on the form 
            titleComboBox.Focus();

            // Clear data on the form and reset defaults 

            deliveryDateMaskedTextBox.Text = DateTime.Now.ToString("MM/dd/yyyy");
            specialOccasionComboBox.SelectedItem = "Birthday";
            titleComboBox.Text = "";
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            streetTextBox.Text = "";
            cityTextBox.Text = "";
            stateMaskedTextBox.Text = "";
            zipCodeMaskedTextBox.Text = "";
            phoneNumberMaskedTextBox.Text = "";
            storePickUpRadioButton.Checked = true;
            extrasListBox.ClearSelected();
            extrasListBox.TopIndex = 0;
            extras = ""; 
            singleRadioButton.Checked = true;
            subtotalSummaryLabel.Text = subtotal.ToString("C");
            totalTaxLabel.Text = salesTaxAmount.ToString("C");
            totalLabel.Text = orderTotal.ToString("C");
            personalizedMessageCheckBox.Checked = false;
            extraList = "";
        }

        // Custom method to update totals on form 

        private void UpdateTotals()
        {
       
            // Local variables reset to sum totals 

            subtotal = 0m;
            extraList = "";
            extraCount = 0; 

            // If block to calculate home delivery pricing 

            if (homeDeliveryRadioButton.Checked)
            {
                subtotal += HOME_DELIVERY;
            }

            // Calculate total based on bundle size 

            if (singleRadioButton.Checked)
            {
                subtotal += SINGLE_BALLOON ;
            }
            else if (halfDozenRadioButton.Checked)
            {
                subtotal += HALF_DOZEN_BALLOON ;
            }
            else if (dozenRadioButton.Checked)
            {
                subtotal += DOZEN_BALLOON;
            }

            // Add personal message price to subtotal 

            if (personalizedMessageCheckBox.Checked)
            {
                subtotal += PERSONALIZED_MESSAGE;
            }

            // Loop through items in extra items listbox and display if selected

            for (int counter = 0; counter < extrasListBox.Items.Count; counter++)
            {

             // Determines if items in the list are selected or not 

             if (extrasListBox.GetSelected(counter))
             {

                extraList += extrasListBox.Items[counter] + "\n"; // Concatenates each item in list with space 
                extraCount++;  // Increment count of variable                      
             }
                    
             }

                // Add selected extra items from list box to subtotal 

                subtotal += extraCount * EXTRAS;

                // Calculate sales tax and total for order

                salesTaxAmount = TAX_RATE * subtotal;
                orderTotal = salesTaxAmount + subtotal;

                // Display totals on the form 

                subtotalSummaryLabel.Text = subtotal.ToString("C");
                totalTaxLabel.Text = salesTaxAmount.ToString("C");
                totalLabel.Text = orderTotal.ToString("C");
            
        }
            
        // Click event handler for clear button 

        private void clearFormButton_Click(object sender, EventArgs e)
        {
            // Call to method ResetForm to reset form to default appearance 

            ResetForm();
        }

        // Click event handler for exit button 
         
        private void exitProgramButton_Click(object sender, EventArgs e)
        {
            // Declare Variable To Hold User Selection To Terminate The Program 

            DialogResult selection;

            // Display Message To Confirm User Selection 

            selection = MessageBox.Show(" Are You Sure You Want To Quit?",
            "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Determines What Happens After User Selection 

            if (selection == DialogResult.Yes)
            {
                this.Close();
            }
            if (selection == DialogResult.No)
            {
                titleComboBox.Focus();
            }
        }

        // Event handler to control what happens when Display summary clicked

        private void displaySummaryButton_Click(object sender, EventArgs e)
        {
            extraList = "";
            extraCount = 0;

            // Loop through items in extra items listbox and display if selected

            for (int counter = 0; counter < extrasListBox.Items.Count; counter ++)
            {
                // Determines if items in the list are selected or not 

                if (extrasListBox.GetSelected(counter))
                {
                    extraList += extrasListBox.Items[counter] + "\n"; // Concatenates each item in list with space 
                    extraCount++;  // Increment count of variable 
                   
                }
                             
            }

            // If block to determine what type of delivery was selected

            if(storePickUpRadioButton.Checked)
            {
                deliveryType = "Store Pick-Up"; 
            }
            else
            {
                deliveryType = "Home Delivery "; 
            }

            // If block to determine what bundle size was selected

            if (singleRadioButton.Checked)
            {
                bundleSize = "Single "; 
            }
            else if (halfDozenRadioButton.Checked)
            {
                bundleSize = "Half-Dozen "; 
            }
            else if (dozenRadioButton.Checked)
            {
                bundleSize = "Dozen "; 
            }

            // Displays error message if user does not enter phone number
                            
            if (phoneNumberMaskedTextBox.MaskCompleted == false)
            {
                MessageBox.Show("User must enter a phone number");
            }

            // Displays error meesage if first name is not entered 

                if (firstNameTextBox.Text != "")
                {
                    // If first name is entered check for last name 

                    if (lastNameTextBox.Text != "")
                    {
                        // Display message box that includes names of selected items

                        MessageBox.Show("Customer Name : " + titleComboBox.Text + firstNameTextBox.Text + " " + lastNameTextBox.Text + "\n" +
                            "Street Name : " + stateMaskedTextBox.Text + " " + "City : " + cityTextBox.Text + " " + "State : " + stateMaskedTextBox.Text + " " + "zip Code : " + zipCodeMaskedTextBox.Text + "\n" +
                            "Customer Phone Number : " + phoneNumberMaskedTextBox.Text + "\n" + "Delivery Date : " + deliveryDateMaskedTextBox.Text + "\n" +
                            "Delivery Type : " + deliveryType + "\n" +
                            "Bundle Size : " + "" + bundleSize + "\n" +
                            "Occasion : " + specialOccasionComboBox.Text + "\n" +
                            "Personalized Message : " + personalMessageTextBox.Text + "\n" +
                            "Subtotal : " + subtotal + "\n" +
                            "Sales Tax : " + salesTaxAmount + "\n" +
                            "Order Total : " + orderTotal + "\n" +
                            "Extra Items:\n" + extraList + "\n", "Bonnie's Balloons Order Summary:", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            else
            {
                MessageBox.Show("User Must Enter a Phone Number, Last Name and First Name" + "\n", "ERROR",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            // Call to method ResetForm to reset form to default appearance 

            ResetForm();
        }

        // Event handler to update totals for bundle size changes 

        private void halfDozenRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            // Call to method update totals to handle check changed event for all bundles 
            UpdateTotals(); 
        }

        // Event handler for delivery type 

        private void storePickUpRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            // Call to update totals method to add delivery charge
        
            UpdateTotals();
        }

        private void extrasListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Call to update totals method to add delivery charge

            UpdateTotals();
        }
    }
}
