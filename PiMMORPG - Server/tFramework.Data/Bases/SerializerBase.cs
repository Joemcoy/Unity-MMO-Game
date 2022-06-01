using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

using tFramework.Interfaces;

namespace tFramework.Data.Bases
{
	using Extensions;
	using Interfaces;
	using Contracts;

	public abstract class SerializerBase<TSerializer>
		where TSerializer : SerializerBase<TSerializer>, new()
	{
		private static TSerializer _instance;
		protected static List<IContract> Contracts;

		protected static ILogger Logger { get { return typeof(TSerializer).GetLogger(); } }
		protected static TSerializer Instance { get { return _instance ?? (_instance = new TSerializer()); } }

		public static void RegisterContract<TContract>(TContract contract) where TContract : IContract
		{
			if (!Contracts.Any(c => c.AssociatedType == contract.AssociatedType))
				Contracts.Add(contract);
			else
				Logger.LogWarning("An contract that associated to type {0} already as been registered!", contract.AssociatedType.Name);
		}

		static IContract GetContract(Type target)
		{
			if (Contracts == null)
			{
				Contracts = new List<IContract>();
				Contracts.Add(new TimeSpanContract());
                Contracts.Add(new DateTimeContract());
                Contracts.Add(new IPEndPointContract());
            }

			return Contracts.FirstOrDefault(c => c.AssociatedType == target);
		}

		protected abstract SerializerElement LoadData<T>(Stream source);
		public static bool Load<T>(ref T item, Stream source)
		{
			try
			{
				var element = Instance.LoadData<T>(source);
				return Deserialize(ref item, element);
			}
			catch (Exception ex)
			{
				Logger.LogFatal(ex);
				return false;
			}
		}

		public static bool Load<T>(T item, Stream source)
		{
			try
			{
				var element = Instance.LoadData<T>(source);
				return Deserialize(ref item, element);
			}
			catch (Exception ex)
			{
				Logger.LogFatal(ex);
				return false;
			}
		}

		static bool Deserialize<T>(ref T item, SerializerElement element)
		{
			object temp = item as object;
			if (Deserialize(typeof(T), ref temp, element))
			{
				item = (T)temp;
				return true;
			}
			return false;
		}

		static bool Deserialize(Type target, ref object item, SerializerElement element)
		{
			try
			{
                if (target.IsClass)
                {
                    if (target.IsArray)
                    {
                        var elementType = target.GetElementType();
                        var array = item as Array;

                        if (array == null || array.Length != element.Childrens.Count)
                            array = Array.CreateInstance(elementType, element.Childrens.Count);

                        for (int i = 0; i < array.Length; i++)
                        {
                            object iElement = null;
                            if (Deserialize(elementType, ref iElement, element.Childrens[i]))
                                array.SetValue(iElement, i);
                            else
                                return false;
                        }
                        item = array;
                    }
                    else if(typeof(IDictionary).IsAssignableFrom(target))
                    {
                        var ga = target.GetGenericArguments();
                        Type keyType = ga[0], valueType = ga[1];

                        var dict = Activator.CreateInstance(target) as IDictionary;
                        foreach(SerializerElement ditem in element.Childrens)
                        {
                            object key = null, value = null;
                            if (!Deserialize(keyType, ref key, ditem.Childrens[0])) return false;
                            else if (!Deserialize(valueType, ref value, ditem.Childrens[1])) return false;

                            dict.Add(key, value);
                        }

                        item = dict;
                    }
                    else if (target == typeof(string))
                        item = Convert.ToString(element.Attributes[0].Value);
                    else
                    {
                        var contract = GetContract(target);
                        if (contract != null)
                            item = contract.Deserialize(element);
                        else
                        {
                            if (item == null)
                                item = Activator.CreateInstance(target);

                            foreach (var attribute in element.Attributes)
                            {
                                var property = target.GetProperty(attribute.Name);
                                var ptarget = property.PropertyType;

                                object value = null;
                                if (ptarget.IsPrimitive)
                                    value = Convert.ChangeType(attribute.Value, ptarget);
                                else if (ptarget.IsEnum)
                                    value = Enum.Parse(ptarget, attribute.Value);
                                else
                                    return false;
                                property.SetValue(item, value, null);
                            }

                            foreach (var children in element.Childrens)
                            {
                                var property = target.GetProperty(children.Name);
                                object value = null;

                                if (Deserialize(property.PropertyType, ref value, children))
                                    property.SetValue(item, value, null);
                                else
                                    return false;
                            }
                        }
                    }
                }
                else if (target.IsPrimitive)
                {
                    item = Convert.ChangeType(element.Attributes[0].Value, target);
                }
                else if (target.IsEnum)
                {
                    item = Enum.Parse(target, element.Attributes[0].Value);
                }
                else if (typeof(ICustomElement).IsAssignableFrom(target))
                {
                    var custom = Activator.CreateInstance(target) as ICustomElement;
                    custom.Deserialize(element);

                    item = custom;
                }                
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogFatal(ex);
				return false;
			}
		}

        protected abstract void WriteData(Stream source, SerializerElement element, string comments = null);
        public static bool Save<T>(T item, Stream source, string comments = null)
        {
            try
            {
				var element = new SerializerElement(typeof(T).Name);
				var oBracket = element.Name.IndexOf('[');

				if (oBracket > -1)
					element.Name = element.Name.Substring(0, oBracket);

                var oAsterisk = element.Name.IndexOf('`');
                if (oAsterisk > -1)
                    element.Name = element.Name.Substring(0, oAsterisk);

				if(LoadElement(element, item))
                	Instance.WriteData(source, element, comments);
				else
					return false;

                return true;
            }
            catch(Exception ex)
            {
                Logger.LogFatal(ex);
                return false;
            }
        }

		protected static bool LoadElement(SerializerElement element, object item)
		{
			if (item == null)
				return false;
            
            try
			{
				var target = item.GetType();
                if (target.IsArray)
                {
                    var array = item as Array;
                    for (int i = 0; i < array.Length; i++)
                    {
                        var children = new SerializerElement("Item");
                        var value = array.GetValue(i);

                        if (!LoadElement(children, value))
                            return false;
                        else
                            element.Childrens.Add(children);
                    }
                }
                else if (typeof(IDictionary).IsAssignableFrom(target))
                {
                    var dict = item as IDictionary;
                    var ga = target.GetGenericArguments();
                    Type keyType = ga[0], valueType = ga[1];

                    foreach(var key in dict.Keys)
                    {
                        var children = new SerializerElement("Item");
                        var kChildren = new SerializerElement("Key");
                        var vChildren = new SerializerElement("Value");

                        var value = dict[key];

                        if (!LoadElement(kChildren, key)) return false;
                        else if (!LoadElement(vChildren, value)) return false;

                        children.Childrens.Add(kChildren);
                        children.Childrens.Add(vChildren);
                        element.Childrens.Add(children);
                    }
                }
                else if (typeof(ICustomElement).IsAssignableFrom(target))
                {
                    var custom = item as ICustomElement;
                    custom.Serialize(element);
                }
                else if (target.IsPrimitive || target == typeof(string))
                    element.Attributes.Add(new SerializerAttribute("Value", Convert.ToString(item)));
                else if (target.IsEnum)
                    element.Attributes.Add(new SerializerAttribute("Value", Enum.GetName(target, item)));
                else
                {
                    var contract = GetContract(target);
                    if (contract != null)
                        contract.Serialize(element, item);
                    else
                    {
                        var properties = target.GetProperties().Where(p => p.CanRead && p.CanWrite);

                        foreach (var property in properties)
                        {
                            var type = property.PropertyType;
                            var value = property.GetValue(item, null);

                            if (type.IsClass)
                            {
                                var children = new SerializerElement(property.Name);
                                if (!LoadElement(children, value))
                                    return false;
                                else
                                    element.Childrens.Add(children);
                            }
                            else if (type.IsPrimitive || type == typeof(string))
                                element.Attributes.Add(new SerializerAttribute(property.Name, Convert.ToString(value)));
                            else if (type.IsEnum)
                                element.Attributes.Add(new SerializerAttribute(property.Name, Enum.GetName(type, value)));
                        }
                    }
                }
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogFatal(ex);
				return false;
			}
		}
	}
}

//		static string PrintElements(SerializerElement[] elements, string prefix = "-")
//		{
//			var data = string.Empty;
//			foreach (var element in elements) {

//				if (element.Childrens.Length > 0)
//					data += PrintElements (element.Childrens, prefix + "-");
//			}
//			return data;
//		}

//        public static bool Load<T>(T item, Stream source)
//        {
//            try
//            {
//                var elements = Instance.LoadData<T>(source);
//				Logger.LogInfo(PrintElements(elements));
//                return Deserialize(item, typeof(T), elements);
//            }
//            catch (Exception e)
//            {
//                Logger.LogFatal(e);
//                return false;
//            }
//        }

//        protected static bool Deserialize(object instance, Type targetType, SerializerElement[] elements)
//        {
//            if (targetType.IsArray)
//            {
//                var elementType = targetType.GetElementType();
//                elements = elements[0].Childrens;

//                var array = instance as Array;
//                if (array.Length != elements.Length)
//                    array = Array.CreateInstance(elementType, elements.Length);

//                for(int i = 0; i < elements.Length; i++)
//                {
//                    var item = Activator.CreateInstance(elementType);
//                    if (Deserialize(item, elementType, elements[i].Childrens))
//                        array.SetValue(item, i);
//                    else
//                        return false;
//                }
//                return true;
//            }
//            else
//            {
//                foreach (var element in elements)
//                {
//                    var property = targetType.GetProperty(element.Name);
//                    if (property != null)
//                    {
//                        object value = Load(property.PropertyType, element);
//                        property.SetValue(instance, value, null);
//                    }
//                    else
//                        Logger.LogWarning("Invalid element '{0}'", element.Name);
//                }
//                return true;
//            }
//        }

//        protected static object Load(Type targetType, SerializerElement element)
//        {
//            object value = null;
//            var contract = GetContract(targetType);

//            if (contract != null)
//                value = contract.Deserialize(element);
//            else if (targetType.IsEnum)
//                value = Enum.Parse(targetType, element.Value);
//            else if (targetType.IsPrimitive || targetType == typeof(string))
//                value = Convert.ChangeType(element.Value, targetType);
//            else if (typeof(ICustomElement).IsAssignableFrom(targetType))
//            {
//                var custom = Activator.CreateInstance(targetType) as ICustomElement;
//                custom.Deserialize(element);

//                value = custom;
//            }
//            else if (targetType.IsArray)
//            {
//                var elementType = targetType.GetElementType();
//                Array values = Array.CreateInstance(elementType, element.Childrens.Length);

//                for (int i = 0; i < element.Childrens.Length; i++)
//                {
//                    var item = Load(elementType, element.Childrens[i]);
//                    values.SetValue(item, i);
//                }
//                value = values;
//            }
//            else
//            {
//                value = Activator.CreateInstance(targetType);
//                if(!Deserialize(value, targetType, element.Childrens))
//                    Logger.LogWarning("Failed to load the element {0}!", element.Name);
//            }
//            return value;
//        }

//        protected abstract void WriteData(Stream source, SerializerElement[] elements);
//        public static bool Save<T>(T item, Stream source)
//        {
//            try
//            {
//                var elements = LoadElements(item);
//                Instance.WriteData(source, elements);

//                return true;
//            }
//            catch(Exception ex)
//            {
//                Logger.LogFatal(ex);
//                return false;
//            }
//        }

//        protected static SerializerElement[] LoadElements(object item)
//        {
//            if (item == null)
//                return new SerializerElement[0];
//            else if (item.GetType().IsArray)
//            {
//                var elements = new SerializerElement[1] { new SerializerElement("Array") };
//                var array = item as Array;

//                elements[0].Childrens = new SerializerElement[array.Length];
//                for (int i = 0; i < array.Length; i++)
//                {
//                    var element = elements[0].Childrens[i] = new SerializerElement("Item");
//                    element.Childrens = LoadElements(array.GetValue(i));
//                }

//                return elements;
//            }
//            else
//            {
//                var itemType = item.GetType();
//                var properties = itemType.GetProperties().Where(p => p.CanRead && p.CanWrite).ToArray();
//                var elements = new SerializerElement[properties.Length];

//                for (int i = 0; i < properties.Length; i++)
//                {
//                    var property = properties[i];
//                    var value = property.GetValue(item, null);

//                    var element = elements[i] = new SerializerElement(property.Name);
//                    if (value != null)
//                    {

//                        if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
//                            element.Value = Convert.ToString(value);
//                        else if (property.PropertyType.IsEnum)
//                            element.Value = Enum.GetName(itemType, value);
//                        else if (typeof(ICustomElement).IsAssignableFrom(property.PropertyType))
//                        {
//                            var custom = value as ICustomElement;
//                            custom.Serialize(element);
//                        }
//                        else if (property.PropertyType.IsArray)
//                        {
//                            var array = value as Array;

//                            element.Childrens = new SerializerElement[array.Length];
//                            for (int j = 0; j < array.Length; j++)
//                            {
//                                var children = element.Childrens[j] = new SerializerElement("Item");
//                                children.Childrens = LoadElements(array.GetValue(j));
//                            }
//                        }
//                        else
//                        {
//                            var contract = GetContract(property.PropertyType);
//                            if (contract != null)
//                                contract.Serialize(element, value);
//                            else
//                                element.Childrens = LoadElements(value);
//                        }
//                    }
//                }

//                return elements;
//            }
//        }
//    }
//}
