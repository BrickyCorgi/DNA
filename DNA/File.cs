// ---------------------------------------------------------------------------
// File name: File.cs
// Project name: DNA
// ---------------------------------------------------------------------------
// Creator’s name and email: Danelle Hennings hennings@goldmail.etsu.edu					
// Creation Date: November 2, 2016		
// ---------------------------------------------------------------------------
namespace DNA
{
    /// <summary>
    /// Used for retrieving any possible data from DNA files and turning this data into
    /// relevant information on child-parent relationships and the sex of the dna
    /// </summary>
    class File
    {
        /// <summary>
        /// Gets or sets the dna elements at each line
        /// </summary>
        /// <value>
        /// array of 5 elements pertaining to columns in DNA file
        /// </value>
        public string[ ] dna
        {
            get; set;
        }
        /// <summary>
        /// Tracks the number of alelle 1 mismatches.
        /// </summary>
        /// <value>
        /// The al1 mismatch.
        /// </value>
        public int al1Mismatch
        {
            get; set;
        }
        /// <summary>
        /// Tracks the number of alelle 2 mismatches.
        /// </summary>
        /// <value>
        /// The al2 mismatch count.
        /// </value>
        public int al2Mismatch
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is child.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is child; otherwise, <c>false</c>.
        /// </value>
        public bool isChild
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the current chromosome
        /// </summary>
        /// <value>
        /// The current chromosome.
        /// </value>
        public int chromosome
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the number for tracking the base pair count for current chromosme
        /// </summary>
        /// <value>
        /// The number of base pairs.
        /// </value>
        public int numPairs
        {
            get; set;
        } 
        /// <summary>
        /// Gets or sets the gender of the DNA file.
        /// </summary>
        /// <value>
        /// string containing "male" or "female" depending on determined sex
        /// </value>
        public string sex
        {
            get; set;
        }
        /// <summary>
        /// Tracks the average number of mismatches for each chromsome
        /// </summary>
        /// <value>
        /// array of 22 averages
        /// </value>
        public double[ ] avgError
        {
            get; set;
        }
        /// <summary>
        /// Default constructor for creating new instances of the File class.
        /// </summary>
        public File()
        {
            dna = new string[5];       //initialize string array of 5 elements regarding current line in dna
            al1Mismatch = 0;
            chromosome = 1;             //initalize chromosome to 1
            numPairs = 0;               //initialize number of base pairs to be incrememented for each line in a chromosome
            al2Mismatch = 0;
            sex = "undetermined";       //starts with an unknown sex for the DNA
            isChild = false;           //assume not the child file
            avgError = new double[22];
        }//end file

        /// <summary>
        /// Gets the elements from the line in the file by uisng whitespace 
        /// to split into a string array of the 5 elements .
        /// </summary>
        /// <param name="line">The line.</param>
        public void GetElements(string line)
        {
            //split up the 5 elements from this line into an array
            dna = line.Split( );  
        }

        /// <summary>
        /// Compares the current line of this file against the one passed and counts the number of mismatches
        /// found for each allele
        /// </summary>
        /// <param name="file">The file this one is compared against</param>
        public void Compare(File file)
        {
            /**First DNA allelle 1 compared to all of Second DNA**/
            //if 1st allele of 1st dna does not equal either alleles of 2nd dna
            if (dna[3] != file.dna[3] && dna[3] != file.dna[4])
            {
                al1Mismatch++;   //increment # of mismatches for allele 1
            }
            //if 2nd allele of 1st dna does not equal either alleles of 2nd dna
            if (dna[4] != file.dna[3] && dna[4] != file.dna[4])
            {
                al2Mismatch++;   //increment # of mismatches for allele 2
            }
        }

        /// <summary>
        /// When switching to the next chromosome this calculates an average for the
        /// number of mismatches out of the number of base pairs for chromsome
        /// It resets the number of mismatches and numPairs and increments to the next chromosome
        /// </summary>
        public void Next( )
        {  
            if (al1Mismatch < al2Mismatch)
            {
                avgError[chromosome - 1] = al1Mismatch / (double)numPairs; 
            }
            else
            {
                avgError[chromosome - 1] = al2Mismatch / (double)numPairs;
            }

            al1Mismatch = 0;
            al2Mismatch = 0;
            numPairs = 0;
            chromosome++;
        }//end Next

        /// <summary>
        /// Changes the isChild boolean value to true for the object if the total
        /// number of allele mismatches is the least among all four comparisons
        /// at each chromosome
        /// </summary>
        /// <param name="file">The file being compared against</param>
        public void Relation(File file)
        {
            int least = int.MaxValue;       //start with least being largest possible integer   

            if (al1Mismatch < least)
            {
                least = al1Mismatch;
                isChild = true;
            }
            else if (al2Mismatch < least)
            {
                least = al2Mismatch;
                isChild = true;
            }
            else if (file.al1Mismatch < least)
            {
                least = file.al1Mismatch;
                file.isChild = true;
            }
            else 
            {
                least = file.al2Mismatch;
                file.isChild = true;
            }
                   
        }
        /// <summary>
        /// Uses 24th chromosome to determine sex of DNA, instead of calculating average of mismatches
        /// on this chromosome, it is used to count number of "0 0" genotypes,
        /// when this method is called it uses this count to verify there are enough of these pairs to determine
        /// it is a male DNA file, and if not then it is female
        /// sets gender property to "male" or "female"
        /// </summary>
        /// <returns>"male" or "female"</returns>
        public void Gender(int numPairs)
        {   
            //if large enough count of 00 pairs
            if (numPairs > 100)
            {   //then male
                sex = "male";
            }
            else
                sex = "female";
        }  
    }//end class file


}//end namespace DNA
