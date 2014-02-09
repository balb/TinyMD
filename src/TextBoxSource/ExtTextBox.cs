using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;
 

namespace ExtendedTextBox
{
    [System.ComponentModel.DefaultBindingProperty("Text")]
    public partial class ExtTextBox : System.Windows.Forms.UserControl
    {

        private bool _mandatory;
        private string _originalText="";
        private TextTypes _textType = TextTypes.String;
        private System.Drawing.Color _ChangedColour;
        private ErrorProvider errorProvider;
        private string lastError = "";

        public event KeyEventHandler TextBoxKeyDown;

        /// <summary>
        /// Possible types for the textbox
        /// </summary>
        public enum TextTypes
        {
            String,
            Int,
            Currency,
            Name
        }


        public ExtTextBox()
        {
            InitializeComponent();
            wfpExtTextBox1.TextChanged += CheckForTextChanged;    
        }

        public event System.Windows.Controls.TextChangedEventHandler BaseTextChanged
        {
            add { wfpExtTextBox1.TextChanged += value; }
            remove { wfpExtTextBox1.TextChanged -= value; }
        }

        /// <summary>
        /// If the current value in the textbox does not match the original value
        /// then display it with a different background
        /// </summary>
        private void CheckForTextChanged(object sender, TextChangedEventArgs e)
        {
            if (wfpExtTextBox1.Text != _originalText)
                wfpExtTextBox1.BackColor = ConvertColor(_ChangedColour);
            else
                wfpExtTextBox1.BackColor = ConvertColor(System.Drawing.SystemColors.Window);

            NameFormat();
        }


        /// <summary>
        /// Add an original text property to the property box
        /// </summary>
        [Category("Appearance")]
        [Description("The background colour of the textbox when it is changed")]
        public System.Drawing.Color ChangedColour
        {
            get { return _ChangedColour; }
            set 
            {
                _ChangedColour = value;
                CheckForTextChanged(this, null);
            }
        }



        /// <summary>
        /// Turn multiline textbox option on/off
        /// </summary>
        [Category("Behaviour")]
        [Description("Turn multiline text wrapping on/off")]
        public bool Wrapping
        {
            get { return wfpExtTextBox1.TextWrapping == System.Windows.TextWrapping.Wrap; }
            set
            {
                if (value)
                    wfpExtTextBox1.TextWrapping = System.Windows.TextWrapping.Wrap;
                else
                    wfpExtTextBox1.TextWrapping = System.Windows.TextWrapping.NoWrap;
            }
        }   


        /// <summary>
        /// Add an original text property to the property box
        /// </summary>
        [Category("Appearance")]
        [Description("The original unchanged value of the TextBox")]
        public string OriginalText
        {
            get { return _originalText; }
            set { _originalText = value; }
        }



        /// <summary>
        /// Force the input to be of a certain type
        /// </summary>
        [Category("Appearance")]
        [Description("The Type accepted by the Textbox")]
        public TextTypes TextType
        {
            get
            { return _textType; }

            set
            {
                _textType = value;

                if (_textType == TextTypes.Int)
                    wfpExtTextBox1.MaxLength = 10;  

                if (_textType == TextTypes.Currency)
                    wfpExtTextBox1.MaxLength = 16;  

            }
        }



        /// <summary>
        /// Force the case of the textbox
        /// </summary>
        [Category("Appearance")]
        [Description("The case of the textbox")]
        public System.Windows.Forms.CharacterCasing TextCase
        {
            get { return ConvertCharCase(wfpExtTextBox1.CharacterCase); }
            set { wfpExtTextBox1.CharacterCase = ConvertCharCase(value); }
        }



        private System.Windows.Forms.CharacterCasing ConvertCharCase(System.Windows.Controls.CharacterCasing theCase)
        {
            switch (theCase)
            {
                case System.Windows.Controls.CharacterCasing.Upper:
                    return System.Windows.Forms.CharacterCasing.Upper;
                case System.Windows.Controls.CharacterCasing.Lower:
                    return System.Windows.Forms.CharacterCasing.Lower;
                default:
                    return System.Windows.Forms.CharacterCasing.Normal;
            }
        }


        private System.Windows.Controls.CharacterCasing ConvertCharCase(System.Windows.Forms.CharacterCasing theCase)
        {
            switch(theCase)
            {
                case System.Windows.Forms.CharacterCasing.Upper:
                    return System.Windows.Controls.CharacterCasing.Upper;
                case System.Windows.Forms.CharacterCasing.Lower:
                    return System.Windows.Controls.CharacterCasing.Lower;
                default:
                    return System.Windows.Controls.CharacterCasing.Normal;
            }
        }






        /// <summary>
        /// Sets the texts maximum length
        /// </summary>
        [Category("Appearance")]
        [Description("The maximum length of the text")]
        public int MaxLength 
        {
            get { return wfpExtTextBox1.MaxLength; }
            set { wfpExtTextBox1.MaxLength = value; }
        }



        /// <summary>
        /// Turn the spell checking on and off
        /// </summary>
        [Category("Appearance")]
        [Description("Enable/disable spell checking")]
        public bool SpellCheck
        {
            get { return wfpExtTextBox1.SpellCheck; }
            set { wfpExtTextBox1.SpellCheck = value; }
        }



        /// <summary>
        /// Allows the textbox to accept the return character
        /// </summary>
        [Category("Appearance")]
        [Description("Allows the return character to be used in the text")]
        public bool AcceptsReturn
        {
            get { return wfpExtTextBox1.AcceptsReturn; }
            set { wfpExtTextBox1.AcceptsReturn = value; }
        }



        /// <summary>
        /// Allows the tab character to work in the textbox
        /// </summary>
        [Category("Appearance")]
        [Description("Allows the tab character to be used in the text")]
        public bool AcceptsTab
        {

            get { return wfpExtTextBox1.AcceptsTab; }
            set { wfpExtTextBox1.AcceptsTab = value; }
        }



        /// <summary>
        /// Enable / disable the textbox
        /// </summary>
        [Category("Appearance")]
        [Description("Enables/disables the control")]
        public new bool Enabled
        {
            get { return wfpExtTextBox1.IsEnabled; }
            set { wfpExtTextBox1.IsEnabled = value; }
        }



        /// <summary>
        /// set the contents of the textbox
        /// </summary>
        [Category("Appearance")]
        [Description("The texts displayed in the textbox")]
        [System.ComponentModel.Bindable(true), Browsable(true)]
        [DefaultValue(typeof(string), "")]
        public override string Text
        {
            get { return wfpExtTextBox1.Text; }
            set 
            {
                CheckForTextChanged(this, null);           
                wfpExtTextBox1.Text = value; 
            }
        }



        /// <summary>
        /// Alignment of the text within the textbox
        /// </summary>
        [Category("Appearance")]
        [Description("The alignment of the text within the textbox")]
        [DefaultValue(typeof(TextAlignment), "Left")]
        public TextAlignment TextAlignment
        {
            get { return wfpExtTextBox1.TextAlignment; }
            set { wfpExtTextBox1.TextAlignment = value; }
        }



        /// <summary>
        /// Sets both the original and Text properties to the given string
        /// </summary>
        /// <param name="theValue"></param>
        [Category("Appearance")]
        [Description("Set the initial value of the textbox")]
        public void SetValue(string theValue)
        {
            this.OriginalText = theValue;
            this.Text = theValue;
            wfpExtTextBox1.BackColor = ConvertColor(System.Drawing.SystemColors.Window);
        }


        private SolidColorBrush ConvertColor(System.Drawing.Color colour)
        {
            System.Windows.Media.Color clr2 = System.Windows.Media.Color.FromArgb(colour.A, colour.R, colour.G, colour.B);
            return (new SolidColorBrush(clr2));

        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool Handled = false;

            switch (_textType)
            {
                //If Int only allow 0 - 9
                case TextTypes.Int:
                    if (!char.IsDigit((char)keyData) && !char.IsControl((char)keyData))
                    {
                        Handled = true;
                    }
                    break;

                //If currency only allow 0-9 and .(only 1)
                case TextTypes.Currency:
                    if (!char.IsDigit((char)keyData) && !char.IsControl((char)keyData))
                    {
                        if (keyData == Keys.OemPeriod && wfpExtTextBox1.Text.IndexOf(".") != -1)
                            Handled = true;
                        else if (!char.IsDigit((char)keyData) && keyData != Keys.OemPeriod)
                            Handled = true;
                    }
                    break;
            }

            if (!Handled)
                base.ProcessCmdKey(ref msg, keyData);
 
            return false;
        }


        private void NameFormat()
        {
            System.Windows.Controls.TextBox tb = wfpExtTextBox1.theTextBox;

            int saveCurrentCursorPosition = tb.SelectionStart;

            tb.Text = tb.Text.Remove(tb.SelectionStart, tb.SelectionLength);

            if (tb.Text.Length <= 1)
                tb.Text = tb.Text.ToUpper();
            else if (tb.Text.Length > 1)
                if (tb.Text.Substring(tb.Text.Length - 2, 1) == " ")
                    tb.Text = tb.Text.Substring(0, tb.Text.Length - 1) + tb.Text[tb.Text.Length - 1].ToString().ToUpper();

            tb.SelectionStart = saveCurrentCursorPosition;
            tb.SelectionLength = 0;
        }


        /// <summary>
        /// Has the text been changed from its initial value?
        /// </summary>
        /// <returns></returns>
        public bool TextHasBeenChanged()
        {
            return wfpExtTextBox1.theTextBox.Text == _originalText;
        }
    }
}