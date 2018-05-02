// ---------------------------------------------------------------------------
// File name: DNA.cs
// Project name: DNA
// ---------------------------------------------------------------------------
// Creator’s name and email: Danelle Hennings hennings@goldmail.etsu.edu					
// Creation Date: November 2, 2016		
// ---------------------------------------------------------------------------
using System;
using System.IO;

namespace DNA
{
    /// <summary>
    /// Uses two text file objects to make comparison among their dna alleles, tallies up 
    /// mismatches, calculates a percent of total match, returns info on the two dna files based
    /// using this and the object properties
    /// </summary>
    class DNA
    {
        /// <summary>
        /// Mains entry point for the program
        /// </summary>
        /// <param name="args">the program, text file1, text file 2</param>
        static void Main(string[ ] args)
        {
            StreamReader sr1 = null,
                         sr2 = null;
            try
            {
                sr1 = new StreamReader(args[1]);     //open file
                sr2 = new StreamReader(args[2]);     //open file
            }
            catch
            {
                Console.WriteLine("Unable to open one or both of the files: " + args[1] + ", " + args[2]);
            }
  
            File f1 = new File();
            File f2 = new File();
            PullDNA(sr1, sr2, f1, f2);
            Console.WriteLine(display(args, f1, f2));   //display DNA data
            Console.ReadLine();

            sr1.Close( );
            sr2.Close( );
        }

        /// <summary>
        /// pulls file in, reads one line, skips any comments or headers, assigns text to text object
        /// </summary>
        /// <param name="sr1">The stream for file1.</param>
        /// <param name="sr2">The stream for file2.</param>
        /// <param name="f1">The file 1.</param>
        /// <param name="f2">The file 2param>
        public static void PullDNA(StreamReader sr1, StreamReader sr2, File f1, File f2)
        {
            string text1,
                    text2;        //one line of text from dna file
            int genderCount1 = 0,   //counts the times "0 0" pair occurs in chromsome 24
                genderCount2 = 0;   

            //while each line of each files is not empty
            while (( text1 = sr1.ReadLine( ) ) != null
                && ( text2 = sr2.ReadLine( ) ) != null)
            {   //FOR EACH BASE PAIR
                //if line is a comment or column header, read in next line
                while (text1.StartsWith("#") || text1.StartsWith("rsid"))
                {   //read in next line of text from stream  
                    text1 = sr1.ReadLine( );
                }//repeat for next file, if 
                while (text2.StartsWith("#") || text2.StartsWith("rsid"))
                {   //read in next line of text from stream  
                    text2 = sr2.ReadLine( );
                }
                //store elements from this line of text into dna elements array
                f1.GetElements(text1);
                f2.GetElements(text2);
                //check only autosomes
                if (int.Parse(f1.dna[1]) < 24)
                {
                    //if next line starts a new chromosome
                    if (f1.chromosome != int.Parse(f1.dna[1]))
                    {
                        f1.Relation(f2);        //find relation among files
                        f1.Next( );
                        f2.Next( );
                    }
                    //increment for each new line (or set of basepairs)
                    f1.numPairs++;
                    f2.numPairs++;

                    f1.Compare(f2);    //compare f1 against f2, increments for mismatch
                    f2.Compare(f1);    //compare f2 against f1 
                }
                else
                {
                    if (int.Parse(f1.dna[1]) == 24)
                    {
                        // number of "0" genotypes found for each file
                        if (f1.dna[3] == "0")
                        {
                            genderCount1++;
                        }
                        if (f2.dna[3] == "0")
                        {
                            genderCount2++;
                        }
                    }
                    //set gender based on "0 0" pairs found in 24th chromosome
                    f1.Gender(genderCount1);
                    f2.Gender(genderCount2);
                }
            }//end while line not empty
        }

        /// <summary>
        /// Displays information about DNA files, uses string[] args to refer
        /// to the specific file names
        /// </summary>
        /// <param name="files">The specific file names as strings.</param>
        /// <param name="f1">File 1</param>
        /// <param name="f2">File 2</param>
        /// <returns></returns>
        public static string display(string[ ] files, File f1, File f2)
        {
            string text = "\n-------------------------------------------------------------\n";
            text += string.Format("\n\t----{0} has a {1}%  match to {2}----      ", files[1].Substring(14, 4), Math.Round(calc(f1), 2), files[2].Substring(14, 4));
            text += "\n-------------------------------------------------------------\n"
                + string.Format("\t {0}: {1}", files[1].Substring(14, 4), f1.sex)
                + "\t\t" + string.Format("{0}: {1}", files[2].Substring(14, 4), f2.sex)
                + "\n-------------------------------------------------------------\n";
            //98.9 is the threshhold for 2 files going from no relation to related
            if (calc(f1) >= 98.9)
            {  
                if (f1.isChild == true)
                {
                    if (calc(f1) < 99)
                    {
                        text += string.Format("***------**{0} is the grandchild of {1}**------***", files[1].Substring(14, 4), files[2].Substring(14, 4));
                    }
                    else
                        text += string.Format("****-------**{0} is the child of {1}**-------****", files[1].Substring(14, 4), files[2].Substring(14, 4));
                }
                else if (f2.isChild == true)
                {
                    if (calc(f2) < 99)
                    {
                        text += string.Format("***------**{0} is the grandchild of {1}**------***", files[2].Substring(14, 4), files[1].Substring(14, 4));
                    }
                    else
                       text += string.Format("****-------**{0} is the child of {1}**-------****", files[2].Substring(14, 4), files[1].Substring(14, 4));
                }
                text += "\n-------------------------------------------------------------\n";
            }
            else
            {
                text += "There is not a parent-child relationship among these DNA files."
                    + "\n-------------------------------------------------------------\n";
            }
            return text;
        }

        /// <summary>
        /// Calculates percentage match against another file
        /// </summary>
        /// <param name="file">The file in comparison</param>
        /// <returns>the percent</returns>
        public static double calc(File file)
        {
            double sum = 0;
            
            foreach (double val in file.avgError)
            {
                sum += val;
            }
            sum = 100 - ( sum / 22 ) * 10;
            return sum;
        }   

        



    }
}
