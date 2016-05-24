using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.BenchMark.Entitiy
{
    [Serializable]
    public class JsonResult
    {
        private int _returncode;
        public int returncode { get { return _returncode; } set { _returncode = value; } }
        private string _message;
        public string message { get { return _message; } set { _message = value; } }
        private School _result;
        public School result { get { return _result; } set { _result = value; } }
    }
    [Serializable]
    public class School
    {
        private string _name;
        public string name { get { return _name; } set { _name = value; } }
        private string _address;
        public string address { get { return _address; } set { _address = value; } }
        private string _phone;
        public string phone { get { return _phone; } set { _phone = value; } }
        private int _tcount;
        public int tcount { get { return _tcount; } set { _tcount = value; } }
        private int _scount;
        public int scount { get { return _scount; } set { _scount = value; } }
        private List<ClassItems> _classitems;
        public List<ClassItems> classitems { get { return _classitems; } set { _classitems = value; } }

        public void Set()
        {
            if (classitems == null) return;
            foreach (var c in classitems)
            {
                if (c.teachers != null)
                {
                    tcount += c.teachers.Count;
                }
                if (c.students != null)
                {
                    tcount += c.students.Count;
                }
            }
        }
    }
    [Serializable]
    public class ClassItems
    {
        private string _grade;
        public string grade { get { return _grade; } set { _grade = value; } }
        private List<Teacher> _teachers;
        public List<Teacher> teachers { get { return _teachers; } set { _teachers = value; } }
        private List<Student> _students;
        public List<Student> students { get { return _students; } set { _students = value; } }
    }
    [Serializable]
    public class Person
    {
        private string _name;
        public string name { get { return _name; } set { _name = value; } }
        private int _age;
        public int age { get { return _age; } set { _age = value; } }
        private sbyte _sex;
        public sbyte sex { get { return _sex; } set { _sex = value; } }
    }
    [Serializable]
    public class Teacher : Person
    {
        private int _id;
        public int id { get { return _id; } set { _id = value; } }
        private string _course;
        public string course { get { return _course; } set { _course = value; } }
        private string _introduce;
        public string introduce { get { return _introduce; } set { _introduce = value; } }
    }
    [Serializable]
    public class Student : Person
    {
        private int _id;
        public int id { get { return _id; } set { _id = value; } }
        private string _fatherName;
        public string fatherName { get { return _fatherName; } set { _fatherName = value; } }
        private string _motherName;
        public string motherName { get { return _motherName; } set { _motherName = value; } }
        private string _introduce;
        public string introduce { get { return _introduce; } set { _introduce = value; } }
    }
}
