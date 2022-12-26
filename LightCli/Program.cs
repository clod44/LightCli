
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;


/*
 * I need to use multiple files to organize my stuff and refactor my code but meh.
 * this library is horrible. do not use it yourself. this is just me trying to learn stuff by myself.
 */



namespace LightCli
{
    class MainClass
    {
        public static int screenWidth = 90;
        public static int screenHeight = 25;
        private static Boolean gameOn = true;

        public static void Main(string[] args)
        {

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;




            LCLI cli = new LCLI(screenWidth, screenHeight);


            //basic game loop for input handling
            while (gameOn)
            {
                clearScreen();

                /*                
                    LCLI_BoilerPlateWindow myWindow = new LCLI_BoilerPlateWindow(
                        xCoords, 
                        yCoords, 
                        width, 
                        height, 
                        borderStyle, 
                        brightness, 
                        "Title", 
                        new string[]{"text1(first line)","text2(force another line)"},
                        textAlign);
                                               
                    myWindow.showOn(LCLI OBJECT HERE);
                */

                LCLI_BoilerPlateWindow w1 = new LCLI_BoilerPlateWindow(3, 3, 30, 15, 1, 2, "AMONGUS", new string[] { "I need to stop procastinating so I can get rich.", "(1) - Exercise", "(2) - Read Books", "(3) - No Fap" }, -1);
                w1.showOn(cli);

                LCLI_BoilerPlateWindow w2 = new LCLI_BoilerPlateWindow(50, 5, 40, 15, 2, 1, "SUSSY", new string[] { "very long centered text:", "", "Positive camber is the angle at which the top of a wheel is tilted outward from the vertical and is designed to improve handling and stability by maintaining tire contact with the road surface when cornering." }, 0);
                w2.showOn(cli);

                LCLI_BoilerPlateWindow w3 = new LCLI_BoilerPlateWindow(20, 10, 40, 13, 3, 1, "Fluids", new string[] { "written by chatgpt", " ", "Fluid dynamics is the study of how fluids (liquids and gases) behave and interact when they are in motion. It involves analyzing the forces that act on fluids and predicting their behavior under various conditions." }, 1);
                w3.showOn(cli);

                LCLI_BoilerPlateWindow w4 = new LCLI_BoilerPlateWindow(10, 18, 20, 10, 3, 0, "Transparent!!", new string[] { "laga luga" }, 0);
                w4.showOn(cli);


                cli.renderScreen();
                read("Your Input");

            }
        }

        public static string read(string title="")
        {
            if(title.Length > 0)
            {
                Console.WriteLine("╔═[ " + title+ " ]" + new string('═', screenWidth - 6 - title.Length));
            }
            else
            {
                Console.WriteLine("╔" + new string('═', screenWidth - 1));
            }
            Console.Write("╚\\\\  ");
            return Console.ReadLine();
        }
        public static void clearScreen()
        {
            Console.Clear();
        }


    }


    //a boilerplate configuration of LCLI_Contents which looks ok.
    //I seriously need to pick a naming technique
    class LCLI_BoilerPlateWindow
    {
        public int x, y, width, height, windowPaletteChoice, windowBrightnessChoice;
        public string title;
        public string[] texts;
        public int textAlign = -1;

        private LCLI_Content mainW;
        private LCLI_Content titleW;
        private LCLI_Content textsW;
        public LCLI_BoilerPlateWindow(int _x, int _y, int _width, int _height, int _windowPaletteChoice, int _windowBrightnessChoice, string _title, string[] _texts, int _textAlign)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            windowPaletteChoice = _windowPaletteChoice;
            windowBrightnessChoice = _windowBrightnessChoice;
            title = _title;
            texts = _texts;
            textAlign = _textAlign;

            //the main window
            mainW = new LCLI_Content(x, y, width, height, windowPaletteChoice, windowBrightnessChoice);

            //the title (if its given)
            if (title.Length > 0)
            {
                int titleBgChoice = Math.Min(mainW.windowBrightnessPalettes.Length - 1, windowBrightnessChoice+1);
                titleW = new LCLI_Content(x, y, width, 3, windowPaletteChoice, titleBgChoice);
                titleW.text=title;
                titleW.textAlign = 0;
                titleW.textPadding = 1;
                //offset stuff and decrease the content for the texts
                y += 2;
                height -= 2;
            }

            //texts
            textsW = new LCLI_Content(x, y, width, height, 0, 0);
            foreach (string text in texts)
            {
                // #breakLine# will be recognized by text wrapping algorithm to create a new line
                textsW.text += "" + text + " #breakLine# ";
            }
            textsW.textAlign = textAlign;

        }


        public void showOn(LCLI cli)
        {
            //order is important!
            cli.addContent(mainW);
            cli.addContent(textsW);
            cli.addContent(titleW);
        }



    }

    //a rectangle that can show a text. wow.
    class LCLI_Content
    {
        public int x, y, width, height, windowPaletteChoice, windowBrightnessChoice;
        public string text = "";
        public int textAlign = -1;
        public int textPadding = 2;
        public string[] windowBrightnessPalettes = { " ", ".", "░", "▒", "▓", "█" };
        private string[,] windowPalettes = {
            {
                " ", " ", " ",
                " ", " ", " ",
                " ", " ", " "
            },
            {
                "╭", "―", "╮",
                "│", " ", "│",
                "╰", "―", "╯"
            },
            {
                "╔", "═", "╗",
                "║", " ", "║",
                "╚", "═", "╝"
            },
            {
                "╭", "―", "╮",
                "│", " ", "║",
                "╰", "═", "╝"
            }
        };
        public string[,] content;

        public LCLI_Content(int _x, int _y, int _width, int _height, int _windowPaletteChoice = 1, int _windowBrightnessChoice = 0)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            windowPaletteChoice = _windowPaletteChoice;
            windowBrightnessChoice = _windowBrightnessChoice;
            content = new string[height, width];

        }

        public void generateContent()
        {
            //1-fill the bg
            //2-add text (1pixel paddinged from borders. eg 6x6 content will have 4x4 area to put the text) 
            //3-add borders


            //fill the bg
            string o = windowBrightnessPalettes[windowBrightnessChoice];
            for (int _y = 0; _y < height; _y++)
            {
                for (int _x = 0; _x < width; _x++)
                {
                    content[_y, _x] = o;
                }
            }


            //process the text
            string[] chunks = SplitIntoChunks(text, width - textPadding * 2);
            for (int i = 0; i < Math.Min(chunks.Length, height - textPadding * 2); i++)
            {
                //align the line
                chunks[i] = AlignString(chunks[i], width - textPadding * 2, textAlign, windowBrightnessPalettes[windowBrightnessChoice][0]);
                //dump it to screen
                for (int j = 0; j < Math.Min(width - textPadding * 2, chunks[i].Length); j++)
                {
                    content[i+textPadding, j+textPadding] = "" + (chunks[i][j]);
                }
            }




            //draw the Borders
            string tl = windowPalettes[windowPaletteChoice, 0];
            string t = windowPalettes[windowPaletteChoice, 1];
            string tr = windowPalettes[windowPaletteChoice, 2];
            string l = windowPalettes[windowPaletteChoice, 3];
            string r = windowPalettes[windowPaletteChoice, 5];
            string bl = windowPalettes[windowPaletteChoice, 6];
            string b = windowPalettes[windowPaletteChoice, 7];
            string br = windowPalettes[windowPaletteChoice, 8];


            //top and bottom borders
            for (int i = 0; i < width; i++)
            {
                content[0, i] = t;
                content[height - 1, i] = b;
            }
            //left and right borders
            for (int i = 0; i < height; i++)
            {
                content[i, 0] = l;
                content[i, width - 1] = r;
            }
            //corders
            content[0, 0] = tl;
            content[0, width - 1] = tr;
            content[height - 1, 0] = bl;
            content[height - 1, width - 1] = br;



        }


        //some string functions that i shamelesly scraped from web
        //simple word wrapping function
        public static string[] SplitIntoChunks(string str, int chunkSize)
        {
            //what we are doing here is, ceate an empty line, keep adding new words to it until its longer than the specified size.
            //then add another line.. etc


            // Split the input string into an array of words
            string[] words = Regex.Split(str, @"\s+");

            List<string> chunks = new List<string>();

            // Initialize a string to store the current chunk
            string chunk = "";
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];

                if (word == "#breakLine#")
                {
                    chunks.Add(chunk);
                    chunk = "";
                    continue;
                }
                // If the current chunk + the current word would exceed the chunk size, add the current chunk to the list and start a new chunk
                if (chunk.Length + word.Length > chunkSize)
                {
                    chunks.Add(chunk);
                    chunk = "";
                }
                //add a whitespace after every word except the last word (white spaces will be ignored while dumping the data to screen)
                chunk += word + (i<words.Length-1 ? " " : "");
            }

            // Add the final chunk to the list
            chunks.Add(chunk);

            //i like arrays
            return chunks.ToArray();
        }

        //string align algorithm.
        public static string AlignString(string str, int length, int alignment = -1, char whiteSpace = ' ')
        {
            // number of spaces to add
            int spaces = length - str.Length + 1;

            // Add spaces to the left or right of the string based on the alignment
            // default: align to left
            switch (alignment)
            {
                case 0:
                    return new string(whiteSpace, Math.Max(spaces / 2 ,1)) + str + new string(whiteSpace, Math.Max(spaces / 2, 1));
                case 1:
                    return new string(whiteSpace, Math.Max(spaces, 1)) + str;
                default:
                    return str + new string(whiteSpace, Math.Max(spaces, 1));
            }
        }



    }



    //cli as in "command line interface"
    //simple data dumping and showing class for "LCLI_Content"s
    class LCLI
    {
        public static int width;
        public static int height;
        public static string[,] content;

        private static List<LCLI_Content> child_contents = new List<LCLI_Content>();

        public LCLI(int _width, int _height)
        {
            width = _width;
            height = _height;
            content = new string[height, width];

        }

        public void addContent(LCLI_Content _newContent)
        {
            child_contents.Add(_newContent);
        }

        private void renderContent(LCLI_Content _newContent)
        {
            //render the _newContent's content (which is a 2 dimensional text array)...
            //... on top of our main content which is also a 2 mensional text array (our ascii screen)
            //_newContent offsets aka content positions
            int _startX = _newContent.x;
            int _startY = _newContent.y;
            int _width = _newContent.width;
            int _height = _newContent.height;

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    //clamp to prevent unnecessary ass pain
                    string newPixelVal = _newContent.content[y, x];
                    if(newPixelVal != " ") content[clampI(y + _startY, 0, height - 1), clampI(x + _startX, 0, width-1)] = newPixelVal;

                }
            }

        }

        public void generateContent()
        {
            //clear screen
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    content[y, x] = " ";
                }
            }

            //make child contents to dump their data on you (the screen)
            foreach (var _content in child_contents)
            {
                _content.generateContent();
                renderContent(_content);
            }
        }

        public void renderScreen()
        {
            //generate the content
            generateContent();

            string output = "";
            for (int y = 0; y < height; y++)
            {
                string col = "";
                for (int x = 0; x < width; x++)
                {
                    col = col + content[y, x];
                }
                col = col + "\n";
                output = output + col;
            }
            Console.WriteLine(output);
        }

        //lagaluga 
        public int clampI(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }
}


