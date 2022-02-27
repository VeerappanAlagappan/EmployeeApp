using ManageEmployee.BAL;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace EmployeeApp
{
    public class AddEmployee : IExecute
    {
        private readonly ILogger<AddEmployee> _logger;
        private readonly IManageEmployee _manageEmployee;


        public AddEmployee(ILogger<AddEmployee> logger, IManageEmployee manageEmployee)
        {
            _logger = logger;
            _manageEmployee = manageEmployee;
            CommandName = Convert.ToInt32(Actions.Add);
        }

        public int CommandName
        {
            get; set;
        }
        /// <summary>
        /// This method executes the action triggered by the user to add any element in the FS(xml)
        /// </summary>
        /// <returns></returns>
        public bool Execute()
        {
            try
            {
                ManageEmployee.BAL.EmployeeRequest employeeReqObj = new ManageEmployee.BAL.EmployeeRequest();
                #region Get User Inputs
                Console.WriteLine("Enter the name of the employee");
                var name = Console.ReadLine();
                ValidateUserInputFields(ref name);
                employeeReqObj.Name = name;

                Console.WriteLine("Enter the Age of the employee");
                var age = Console.ReadLine();
                ValidateUserInputFields(ref age);
                employeeReqObj.Age = age;
                Console.WriteLine("Enter the Designation of the employee");
                var designation = Console.ReadLine();
                ValidateUserInputFields(ref designation);
                employeeReqObj.Designation = designation;
                Console.WriteLine("Do you want to enter any other details(Type y/yes)?");
                var readInput = Console.ReadLine();
                #endregion
                if (readInput.ToString().ToUpper() == "Y" || readInput.ToString().ToUpper() == "YES")
                {
                    Console.WriteLine("Enter other fields of the employee you want to add in comma seperated");
                    var readUserInput = Console.ReadLine();
                    var getUserInputArray = readUserInput.Split(',');
                    List<dynamic> lstDynamic = new List<dynamic>();

                    foreach (var up in getUserInputArray)
                    {
                        Dictionary<string, string> dictKey = new Dictionary<string, string>();
                        if (up.ToUpper() == "ADDRESS")
                        {
                            Console.WriteLine($"Enter Employee {up}");
                            Console.WriteLine($"Enter Door Number");
                            var doorNo = Console.ReadLine();
                            ValidateUserInputFields(ref doorNo);
                            dictKey.Add("doorNo", doorNo);
                            Console.WriteLine($"Enter Street");
                            var street = Console.ReadLine();
                            ValidateUserInputFields(ref street);
                            dictKey.Add("street", street);
                            Console.WriteLine($"Enter City");
                            var city = Console.ReadLine();
                            ValidateUserInputFields(ref city);
                            dictKey.Add("city", city);
                            Console.WriteLine($"Enter State");
                            var state = Console.ReadLine();
                            ValidateUserInputFields(ref state);
                            dictKey.Add("state", state);
                            Console.WriteLine($"Enter Pincode");
                            var pincode = Console.ReadLine();
                            ValidateUserInputFields(ref pincode);
                            dictKey.Add("pincode", pincode);
                            //Create dynamic classes based on user's decision to add extra fields and map it with Employee object
                            dynamic s1 = MyTypeBuilder.CreateNewObject(up.ToLower(), dictKey);
                            s1.doorNo = dictKey["doorNo"];
                            s1.street = dictKey["street"];
                            s1.city = dictKey["city"];
                            s1.state = dictKey["state"];
                            s1.pincode = dictKey["pincode"];
                            lstDynamic.Add(s1);

                        }
                        else if (up.ToUpper() == "QUALIFICATION")
                        {
                            Console.WriteLine($"Enter Employee {up}");
                            Console.WriteLine($"Enter your Highest Graduation");
                            var degree = Console.ReadLine();
                            ValidateUserInputFields(ref degree);
                            dictKey.Add("degree", degree);
                            dynamic s1 = MyTypeBuilder.CreateNewObject(up.ToLower(), dictKey);
                            s1.degree = dictKey["degree"];
                            lstDynamic.Add(s1);

                        }


                    }

                    return this._manageEmployee.AddEmployees(lstDynamic, employeeReqObj, Common.FilePath);
                }
                else
                {
                    return this._manageEmployee.AddEmployees(null, employeeReqObj, Common.FilePath);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {MethodBase.GetCurrentMethod().Name} || Error Message:{ex.Message} || Stack Trace:{ex.StackTrace}");
                throw ex;
            }

        }

        #region Helper Methods
        private static void ValidateUserInputFields(ref string fields)
        {
            try
            {
                if (string.IsNullOrEmpty(fields))
                {
                    Console.WriteLine("The Entered field is manadatory, please enter again");
                    fields = Console.ReadLine();
                    ValidateUserInputFields(ref fields);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

    }

    // Code to Create Dynamic Classes
    public class FieldDescriptor
    {
        public FieldDescriptor(string fieldName, Type fieldType)
        {
            FieldName = fieldName;
            FieldType = fieldType;
        }
        public string FieldName { get; }
        public Type FieldType { get; }
    }

    public static class MyTypeBuilder
    {
        public static object CreateNewObject(string className, Dictionary<string, string> properties)
        {
            var myTypeInfo = CompileResultTypeInfo(className, properties);
            var myType = myTypeInfo.AsType();
            var myObject = Activator.CreateInstance(myType);

            return myObject;
        }

        public static TypeInfo CompileResultTypeInfo(string className, Dictionary<string, string> properties)
        {
            TypeBuilder tb = GetTypeBuilder(className);
            tb.SetParent(typeof(ManageEmployee.DAL.Employee));
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            var yourListOfFields = new List<FieldDescriptor>();
            foreach (var props in properties)
            {
                yourListOfFields.Add(new FieldDescriptor(props.Key, typeof(string)));
            }
            //{
            //    new FieldDescriptor("YourProp1",typeof(string)),
            //    new FieldDescriptor("YourProp2", typeof(string))
            //};
            foreach (var field in yourListOfFields)
                CreateProperty(tb, field.FieldName, field.FieldType);

            TypeInfo objectTypeInfo = tb.CreateTypeInfo();
            return objectTypeInfo;
        }

        private static TypeBuilder GetTypeBuilder(string className)
        {
            var typeSignature = className;
            var an = new AssemblyName(typeSignature);
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString()), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    null);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }



}