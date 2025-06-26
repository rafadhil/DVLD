using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLayer
{
    public class Person
    {
        public enum enMode { AddNew = 1, Update = 2, Delete = 3 }
        public enum enGender { Male = 0, Female = 1 }
        public int PersonID { get; private set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public enGender Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }
        public enMode Mode;


        public Person()
        {
            PersonID = -1;
            NationalNo = null;
            FirstName = null;
            SecondName = null;
            ThirdName = null;
            LastName = null;
            DateOfBirth = DateTime.MinValue;
            Gender = 0;
            Address = null;
            Phone = null;
            Email = null;
            NationalityCountryID = -1;
            ImagePath = null;
            Mode = enMode.AddNew;

        }

        private Person(int PersonID, string NationalNo,
            string FirstName, string SecondName, string ThirdName,
            string LastName, DateTime DateOfBirth, enGender Gender,
            string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {
            Mode = enMode.Update;
            this.PersonID = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
        }

        public String GetFullName()
        {
            return FirstName + " " + SecondName + " " +
                (String.IsNullOrEmpty(ThirdName) ? "" : ThirdName) + " " +
                LastName;
        }

        public static DataTable GetAllPersons()
        {
            return PersonData.GetAllPersons();
        }

        //ctrl + k + c => comment out selected lines
        //ctrl + k + u => uncomment out selected lines

        public static DataTable GetPersonsByPersonIDLike(int PersonID)
        {

            return PersonData.GetPersonsByPersonIDLike(PersonID);
        }

        public static Person GetPersonByID(int PersonID)
        {
            String NationalNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.MaxValue;
            bool Gender = false;
            int NationalityCountryID = -1;
            if (PersonData.GetPersonByID(PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth,
                ref Gender, ref Address, ref Phone, ref Email,
                ref NationalityCountryID, ref ImagePath))
            {
                enGender gender = Gender ? enGender.Female : enGender.Male;
                return new Person(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
                return null;
        }

        public static Person GetPersonByNationalNumber(String NationalNumber)
        {
            String FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.MaxValue;
            bool Gender = false;
            int NationalityCountryID = -1, PersonID = -1;

            if (PersonData.GetPersonByNationalNumber(NationalNumber, ref PersonID , ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth,
                ref Gender, ref Address, ref Phone, ref Email,
                ref NationalityCountryID, ref ImagePath))
            {
                enGender gender = Gender ? enGender.Female : enGender.Male;
                return new Person(PersonID, NationalNumber, FirstName, SecondName, ThirdName, LastName, DateOfBirth, gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
                return null;
        }

        public static bool IsPersonExist(String NationalNo)
        {
            return PersonData.IsPersonExist(NationalNo);
        }

        public bool DeletePerson()
        {
            if (PersonData.DeletePerson(PersonID))
            {
                this.Mode = enMode.Delete;
                return true;
            }
            else
                return false;
        }

        public static bool DeletePerson(int PersonID)
        {
            return PersonData.DeletePerson(PersonID);
        }
        public bool Save()
        {
            if (Mode == enMode.AddNew)
            {
                PersonID = PersonData.AddNewPerson(NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth,
                    Gender == enGender.Female ? true : false, Address, Phone, Email, NationalityCountryID, ImagePath);

                if (PersonID != -1)
                {
                    Mode = enMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else if (Mode == enMode.Update)
            {
                return PersonData.UpdatePerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth,
                    Gender == enGender.Female ? true : false, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
            {
                return false;
            }
        }

        public static DataTable GetPersonsByCountryID(int CountryID)
        {
            return PersonData.GetPersonsByCountryID(CountryID);
        }

        public static DataTable GetPersonsByNationalNumberLike(String NationalNumber)
        {
            return PersonData.GetPersonsByNationalNumberLike(NationalNumber);
        }

        public static DataTable GetPersonsByFirstNameLike(String FirstName)
        {
            return PersonData.GetPersonsByFirstNameLike(FirstName);
        }
        public static DataTable GetPersonsBySecondNameLike(String SecondName)
        {
            return PersonData.GetPersonsBySecondNameLike(SecondName);
        }
        public static DataTable GetPersonsByThirdNameLike(String ThirdName)
        {
            return PersonData.GetPersonsByThirdNameLike(ThirdName);
        }
        public static DataTable GetPersonsByLastNameLike(String LastName)
        {
            return PersonData.GetPersonsByLastNameLike(LastName);
        }
        public static DataTable GetPersonsByGender(enGender Gender)
        {
            bool Gen = Gender == enGender.Female ? true : false;
            return PersonData.GetPersonsByGender(Gen);
        }

        public static DataTable GetPersonsByEmailLike(String Email)
        {
            return PersonData.GetPersonsByEmailLike(Email);
        }

        public static DataTable GetPersonsByPhoneLike(String PhoneNumber)
        {
            return PersonData.GetPersonsByPhoneike(PhoneNumber);
        }

        public bool HasUserAccount()
        {
            return HasUserAccount(this.PersonID);
        }

        public static bool HasUserAccount(int PersonID)
        {
            return PersonData.HasUserAccount(PersonID);
        }
    }
}
