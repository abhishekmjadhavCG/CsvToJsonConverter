using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToJsonConverter.Process.Core.Entity
{
    public class Customer
    {
        public string CUSTOMER_UNIQUE_ID { get; set; } = "";
        public string CUSTOMER_SOURCE_REF_ID { get; set; } = "";
        public string CUSTOMER_INTERMEDIARY_REF_ID { get; set; } = "";
        public string PERSON_TITLE { get; set; } = "";
        public string FIRST_NAME { get; set; } = "";
        public string LAST_NAME { get; set; } = "";
        public string SUFFIX { get; set; } = "";
        public string CUSTOMER_NAME { get; set; } = "";
        public string CUSTOMER_NAME_1 { get; set; } = "";
        public string CUSTOMER_NAME_2 { get; set; } = "";
        public string CUSTOMER_NAME_3 { get; set; } = "";
        public string CUSTOMER_NAME_4 { get; set; } = "";
        public string REGISTERED_NUMBER { get; set; } = "";
        public string DATE_OF_BIRTH { get; set; } = "";
        public string ADDRESS { get; set; } = "";
        public string ZONE { get; set; } = "";
        public string POSTAL_CODE { get; set; } = "";
        public string COUNTRY_OF_RESIDENCE { get; set; } = "";
        public string COUNTRY_OF_ORIGIN { get; set; } = "";
        public string NATIONALITY_CODE { get; set; } = "";
        public string GENDER_CODE { get; set; } = "";
        public string EMPLOYEE_FLAG { get; set; } = "";
        public string OCCUPATION { get; set; } = "";
        public string ACQUISITION_DATE { get; set; } = "";
        public string CANCELLED_DATE { get; set; } = "";
        public string CUSTOMER_TYPE_CODE { get; set; } = "";
        public string LAST_UPDATED_TIMESTAMP { get; set; } = "";
        public string COMPLEX_STRUCTURE { get; set; } = "";
        public string BLACK_LISTED_FLAG { get; set; } = "";
        public string COMPANY_CODE { get; set; } = "";
        public string ORG_UNIT_CODE { get; set; } = "";
        public string DOMAIN { get; set; } = "";
        public string COMMENTS { get; set; } = "";
        public string PEP_FLAG { get; set; } = "";
        public string VERSION { get; set; } = "";
        public string UPDATED_DATE { get; set; } = "";
        public string CREATION_DATE { get; set; } = "";
        public string SOURCE_SYSTEM { get; set; } = "";
        public string SENSITIVE_CUSTOMER_FLAG { get; set; } = "";
    }
}
