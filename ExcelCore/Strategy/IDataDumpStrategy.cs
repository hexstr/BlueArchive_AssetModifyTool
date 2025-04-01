using ExcelCore.Cryptography;
using ExcelCore.Utility;
using Google.FlatBuffers;
using System.Text.Json.Nodes;

namespace ExcelCore.Strategy
{
    public class IDataDumpStrategy
    {
        public bool IsGlobal { get; set; }

        public virtual JsonObject Dump(object data_list, NormalTable dumper)
        {
            var jobject = new JsonObject();
            var obj_type = data_list.GetType();
            foreach (var property in obj_type.GetProperties())
            {
                if (property.CanRead)
                {
                    if (property.PropertyType == typeof(ByteBuffer))
                    {
                        continue;
                    }

                    var name = property.Name;

                    if (property.PropertyType == typeof(string))
                    {
                        var value = property.GetValue(data_list);
                        if (value != null)
                        {
                            var str = value.ToString();
                            str = dumper.GetString(str);
                            if (opencc.Loaded)
                            {
                                str = opencc.Convert(str);
                                str = str.Translate();
                            }
                            jobject.Add(name, str);
                        }
                    }

                    else if (property.PropertyType == typeof(long))
                    {
                        var value = (long)property.GetValue(data_list);
                        value = dumper.GetLong(value);
                        jobject.Add(name, value);
                    }

                    else if (property.PropertyType == typeof(int))
                    {
                        if (property.Name.EndsWith("Length"))
                        {
                            if (obj_type.GetMethod($"Add{property.Name}") != null)
                            {
                                var value = (int)property.GetValue(data_list);
                                value = dumper.GetInt(value);
                                jobject.Add(name, value);
                            }
                            else
                            {
                                var value = (int)property.GetValue(data_list);
                                jobject.Add(name, value);

                                var contained_element_name = property.Name[..^6];
                                if (value == 0)
                                {
                                    jobject.Add(contained_element_name, new JsonArray());
                                }
                                else
                                {
                                    var get_element_method = obj_type.GetMethod(contained_element_name);
                                    var elements = new JsonArray();
                                    for (int i = 0; i < value; ++i)
                                    {
                                        var result = get_element_method.Invoke(data_list, [i]);
                                        var return_type_name = get_element_method.ReturnType.Name;
                                        if (get_element_method.ReturnType.BaseType.Name == "Enum")
                                        {
                                            return_type_name = "Int32";
                                        }
                                        switch (return_type_name)
                                        {
                                            case "Int32":
                                                elements.Add(dumper.GetInt((int)result));
                                                break;
                                            case "UInt32":
                                                elements.Add(dumper.GetUInt((uint)result));
                                                break;
                                            case "Int64":
                                                elements.Add(dumper.GetLong((long)result));
                                                break;
                                            case "String":
                                                elements.Add(dumper.GetString(result.ToString()));
                                                break;
                                            case "Single":
                                                elements.Add(dumper.GetFloat((float)result));
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    jobject.Add(contained_element_name, elements);
                                }
                            }
                        }
                        else
                        {
                            var value = (int)property.GetValue(data_list);
                            value = dumper.GetInt(value);
                            jobject.Add(name, value);
                        }
                    }

                    else if (property.PropertyType == typeof(uint))
                    {
                        var value = (uint)property.GetValue(data_list);
                        value = dumper.GetUInt(value);
                        jobject.Add(name, value);
                    }

                    else if (property.PropertyType == typeof(float))
                    {
                        var value = (float)property.GetValue(data_list);
                        value = dumper.GetFloat(value);
                        jobject.Add(name, value);
                    }

                    else if (property.PropertyType == typeof(double))
                    {
                        throw new NotImplementedException();
                    }

                    else if (property.PropertyType == typeof(bool))
                    {
                        var value = (bool)property.GetValue(data_list);
                        jobject.Add(name, value);
                    }

                    else if (property.PropertyType.IsValueType)
                    {
                        var value = (int)property.GetValue(data_list);
                        value = dumper.GetInt(value);
                        jobject.Add(name, value);
                    }

                    else
                    {
                        var value = property.GetValue(data_list).ToString();
                        jobject.Add(name, value);
                    }
                }
            }
            return jobject;
        }
    }
}
