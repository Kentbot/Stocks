using System;
using System.Net;
using System.IO;

namespace Stocks
{
	class MainClass
	{
		private static readonly string directory = 
			"C:\\Users\\Kent\\Desktop\\StockInfo\\";

		private static readonly string baseURL = 
			"http://ichart.finance.yahoo.com/table.csv?";

		private static readonly string[] exchanges = 
			{"nyse", "nasdaq", "amex"};

		private static readonly string[] tickers = getSymbols();
		private static readonly string[] targets = createURLs(baseURL, tickers);
		private static readonly string[] files = createFiles(directory, tickers);

		public static void Main (string[] args)
		{
			WebClient wc = new WebClient ();

			for (int i = 0; i < targets.Length; i++) {
				try {
					wc.DownloadFile (targets[i], files[i]);
				} catch (Exception e) {
					Console.WriteLine(e);
				}
			}
		}

		private static string[] createURLs(string baseURL, string[] tickers) {
			int length = tickers.Length;
			string[] targets = new string[length];
			for (int i = 0; i < length; i++) {
				targets[i] = baseURL + "s=" + tickers[i];
			}
			return targets;
		}

		/**
		 * Creates the directory if it doesn't exist, and creates the files if
		 * they don't exist.  Then returns the list of files that correspond to each
		 * ticker for use in the REST GET request.
		 */
		private static string[] createFiles(string dir, string[] tickers) {
			string directory = dir +
				+ System.DateTime.Today.Day + "_" 
				+ System.DateTime.Today.Month + "_" 
				+ System.DateTime.Today.Year;
			int numFiles = tickers.Length;
			string[] fileNames = new string[numFiles];

			if (!Directory.Exists (directory)) {
				Directory.CreateDirectory(directory);
			}

			for (int i = 0; i < numFiles; i++) {
				fileNames[i] = directory + tickers[i] + ".csv";
				if(!File.Exists (fileNames[i])){
					File.Create(fileNames[i]);
				}
//				if(tickers[i] != null) {
//					Console.WriteLine (fileNames[i] + " " + i);
//				}
			}

			return fileNames;
		}

		private static string[] getSymbols() {
			int numTickers = getNumTickers ();
			string[] symbols = new string[numTickers];
			Console.WriteLine ("Num Tickers: " + numTickers);

			int counter = 0;
			foreach (string file in exchanges) {
				StreamReader sr = new StreamReader (directory + file + ".txt");
				char[] currentSymbol;
				string line;
				while ((line = sr.ReadLine ()) != null) {
					currentSymbol = line.ToCharArray();
					bool validLine = true;
					
					for(int i = 0; i < currentSymbol.Length; i++) {
						if (!Char.IsLetter (currentSymbol[i])) {
							validLine = false;
						}
					}
					
					if (validLine) {
						symbols[counter++] = line;
					}
				}
			}

			return symbols;
		}

		private static int getNumTickers() {
			int counter = 0;
			foreach (string file in exchanges) {
				StreamReader sr = new StreamReader (directory + file + ".txt");
				char[] currentSymbol;
				string line;
				while ((line = sr.ReadLine ()) != null) {
					currentSymbol = line.ToCharArray();
					bool validLine = true;
					
					for(int i = 0; i < currentSymbol.Length; i++) {
						if (!Char.IsLetter (currentSymbol[i])) {
							validLine = false;
						}
					}

					if (validLine) {
						counter++;
					}
				}
			}
			return counter;
		} 
	}
}
