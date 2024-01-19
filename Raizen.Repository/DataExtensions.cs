using System;
using System.Collections.Generic;
using System.Data;

namespace Raizen.Repository
{
    public static class DataExtensions
    {
        public static IEnumerable<RootEntity> Materialize<RootEntity>(this IDataReader reader, Func<IDataRecord, RootEntity> materializer)
        {
            var list = new LinkedList<RootEntity>();
            while (reader.Read())
            {
                list.AddLast(materializer(reader));
            }
            return list;
        }

        public static IEnumerable<RootEntity> FetchObjects<RootEntity>(IDbCommand command, Func<IDataRecord, RootEntity> materializer)
        {
            IEnumerable<RootEntity> outcome = null;
            using (var reader = command.ExecuteReader())
            {
                outcome = reader.Materialize(materializer);
            }
            return outcome;
        }

        /// <summary>
        /// Returns a value for a field in the given data record.
        /// </summary>
        /// <typeparam name="T">Expected type of the field.</typeparam>
        /// <param name="record">Current data record.</param>
        /// <param name="name">Field name.</param>
        /// <returns>Field value.</returns>
        public static T Field<T>(this IDataRecord record, string name)
        {
            int ordinal = record.GetOrdinal(name);
            return record.Field<T>(ordinal);
        }

        /// <summary>
        /// Returns a value for a field in the given data record.
        /// </summary>
        /// <typeparam name="T">Expected type of the field.</typeparam>
        /// <param name="record">Current data record.</param>
        /// <param name="ordinal">Field ordinal.</param>
        /// <returns>Field value.</returns>
        public static T Field<T>(this IDataRecord record, int ordinal)
        {
            object value = record.IsDBNull(ordinal) ? default(T) : record.GetValue(ordinal);
            bool isNullable = default(T) == null;

            try
            {
                if (typeof(T).IsEnum && value is string)
                {
                    return (T)Enum.ToObject(typeof(T), char.Parse(value.ToString()));
                }
                else if (typeof(T) == typeof(char) && value is string)
                {
                    value = char.Parse(value.ToString());
                    return (T)value;
                }
                else if ((value is decimal || value is double || value is float) && !isNullable)
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                else
                {
                    return (T)value;
                }

            }
            catch (Exception ex)
            {
                throw new InvalidCastException(string.Format("{{Field:{0},Value:{1}}}", record.GetName(ordinal), value), ex);
            }
        }
    }
}
