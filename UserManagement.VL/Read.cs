using System;
using System.Collections.Generic;
using System.IO;

namespace UserManagement.VL
{
    public class Read
    {
        private View _view;

        public Read(View view)
        {
            _view = view;
        }
        
        public string GetInput()
        {
            return Console.ReadLine();
        }

        public string GetUserAnswer(string action)
        {
            _view.DisplayActionRequest(action);
            return GetInput();
        }

        public DateTime GetDate(string action)
        {
            DateTime date;
            string input;
            do
            {
                _view.DisplayActionRequest(action);
                input = GetInput();

            } while (!DateTime.TryParse(input, out date));
            return date;
        }

        public  void ChangeConsoleInput(StringReader stringReader)
        {
            const int notAvailableCharacters = -1;
            if (stringReader.Peek() != notAvailableCharacters) Console.SetIn(stringReader);
        }

        public string GetNotEmptyString()
        {
            string input = "";
            while (string.IsNullOrWhiteSpace(input))
            {
                input = GetInput();
                if (string.IsNullOrWhiteSpace(input))
                {
                    _view.DisplayError("Please, provide not empty data");
                }
            }
            return input;
        }

        public int GetNumberFromString()
        {
            string stringNumber = "";
            int number;
            while (!int.TryParse(stringNumber, out number))
            {
                stringNumber = GetNotEmptyString();
                if(!int.TryParse(stringNumber, out number))
                {
                    _view.DisplayError("Please, provide numeric data");
                }
            }
            return number;
        }
    }
}
