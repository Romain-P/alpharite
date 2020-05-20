using System;
using System.Linq;
using System.Reflection;

namespace AlphaRite.sdk {
    public static class Reflection {
        public class RecursiveFieldResolve<TFrom, TTo> where TFrom : class where TTo : class {
            private object _instance;
            private Type _type;
            private FieldInfo _field;
            
            public RecursiveFieldResolve(object instance) {
                _instance = instance as TFrom;
                _type = typeof(TFrom);
            }

            public RecursiveFieldResolve<TFrom, TTo> deep<TProxyType>(string proxyFieldName) where TProxyType : class {
                _field = _type.getField(proxyFieldName);
                _instance = _field.GetValue(_instance) as TProxyType;

                var cache = _type;
                _type = typeof(TProxyType);

                if (_instance == null)
                    Alpharite.println("Error while resolving field {0} (type {1}) inside class {2}",
                        proxyFieldName, _type, cache);

                return this;
            }

            public TTo deepAndResolve(string resolvedFieldName) {
                deep<TTo>(resolvedFieldName);
                return resolve();
            }

            public TTo resolve() {
                return _instance as TTo;
            }
        }

        public static RecursiveFieldResolve<TFrom, TTo> proxy<TFrom, TTo>(object instance) where TFrom : class where TTo : class {
            return new RecursiveFieldResolve<TFrom, TTo>(instance);
        }
        
        public static FieldInfo getField<T>(string name) {
            return typeof(T).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static Type getFieldType<T>(string name) {
            return getField<T>( name).GetType();
        }

        public static V getFieldValue<T, V>(object instance, string name) where T : class where V : class {
            return getField<T>(name).GetValue(instance) as V;
        }
        
        public static FieldInfo getField(this Type self, string name) {
            return self.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static Type getFieldType(this Type self, string name) {
            return getField( self, name).GetType();
        }

        public static T getFieldValue<T>(this Type self, object instance, string name) where T : class {
            return getField(self, name).GetValue(instance) as T;
        }
        
        public static FieldInfo getField(this Object self, string name) {
            return self.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static Type getFieldType(this Object self, string name) {
            return getField( self, name).GetType();
        }

        public static T getFieldValue<T>(this Object self, string name) where T : class {
            return getField(self, name).GetValue(self) as T;
        }
        
        public static PropertyInfo getProperty(this Object self, string name) {
            return self.GetType().GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static Type getPropertyType(this Object self, string name) {
            return getProperty( self, name).GetGetMethod().GetType();
        }

        public static T getPropertyValue<T>(this Object self, string name) where T : class {
            return (T) self.GetType().GetProperty(name).GetValue(self, null);
        }

        public static MethodInfo getMethod(this Type self, string methodName) {
            return self.GetMethod(methodName, 
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }
        
        public static PropertyInfo getProperty(this Type self, string propertyName) {
            return self.GetProperty(propertyName, 
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public static string format(this string self, params object[] args) {
            return string.Format(self, args);
        }
        
        public static bool call<T>(string name, params object[] args) {
            var function = typeof(T).GetMethod(name,args.Select(x => x.GetType()).ToArray());
            return (bool) function.Invoke(null, args);
        }
    }
}