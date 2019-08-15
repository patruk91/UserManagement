using System;

namespace UserManagement.VL
{
    public class Read
    {
        private View _view;

        public Read(View view)
        {
            _view = view;
        }

        private string GetInput()
        {
            return Console.ReadLine();
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
