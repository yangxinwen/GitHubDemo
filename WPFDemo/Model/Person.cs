using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WPFDemo.Model
{
  public class Person: IDropable
    {
        public string Name { get; set; }

        public ObservableCollection<Person> Childs { get; set; } = new ObservableCollection<Person>();

        public void Add(object obj)
        {
            Childs.Add(obj as Person);
        }

        public void Remove(object obj)
        {
            Childs.Remove(obj as Person);
        }
    }
}
