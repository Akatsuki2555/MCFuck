using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCFuck {
    public partial class Form1 : Form {
        private string OptionsTXTPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                         "\\.minecraft\\options.txt";
        public Form1() {
            InitializeComponent();
            // if the file doesn't exist, show an error and exit
            if (!File.Exists(OptionsTXTPath)) {
                MessageBox.Show("options.txt not found. Please make sure you have Minecraft installed.");
                Environment.Exit(0);
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            // open a fileSaveDialog and copy the file afterwards to the selected path
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text File|*.txt";
            saveFileDialog.Title = "Save options.txt";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "") {
                File.Copy(OptionsTXTPath, saveFileDialog.FileName, true);
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            // open a fileOpenDisalog and copy the file afterwards to options.txt
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text File|*.txt";
            openFileDialog.Title = "Open options.txt";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "") {
                File.Copy(openFileDialog.FileName, OptionsTXTPath, true);
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            // ask the user if they really want to do it
            var result = MessageBox.Show("Are you sure you want to do this? This will modify your options.txt file and create utter chaos", "Are you sure?", MessageBoxButtons.YesNo);
            // if the answer is no, exit
            if (result == DialogResult.No) {
                return;
            }

            // read the file as lines
            var lines = File.ReadAllLines(OptionsTXTPath);
            // split them in format {key}:{value}
            var options = lines.Select(line => line.Split(':')).ToDictionary(split => split[0], split => split[1]);
            var modifiedOptions = new Dictionary<string, string>();
            // go through each key
            var modifications = 0;
            var random = new Random();
            foreach (var key in options.Keys) {
                // if the key is version, leave it unmodified and continue
                if (key == "version") {
                    modifiedOptions[key] = options[key];
                    continue;
                }

                // if the value is true or false, flip it and continue
                if (options[key] == "true" || options[key] == "false") {
                    modifiedOptions[key] = options[key] == "true" ? "false" : "true";
                }

                // if the value is a number like 0.0, 0.5 and so on, randomize it with no limits
                if (float.TryParse(options[key], out var value)) {
                    float newRandom = random.Next();
                    newRandom /= 10000;
                    modifiedOptions[key] = newRandom.ToString();
                }

                // if it's not present in the modified options, copy it unmodified
                if (!modifiedOptions.ContainsKey(key)) {
                    modifiedOptions[key] = options[key];
                    continue;
                }
                modifications++;
            }
            // save the new options.txt file :)
            File.WriteAllLines(OptionsTXTPath, modifiedOptions.Select(option => $"{option.Key}:{option.Value}"));


            // show a message box of how many modifications were made
            MessageBox.Show($"Modified {modifications} options.");
            // show a message box to enjoy the utter chaos
            MessageBox.Show("Enjoy the utter chaos! :) :)");
        }
    }
}
