using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThiHuong.Framework.Constants
{
    public class StatusConstant
    {
        public const string PUBLIC_EXAM = "PUBLIC";
        public const string PENDING_EXAM = "PENDING";
        public const string CLOSED_EXAM = "CLOSED";


        public const string PUBLIC_STAGE = "PUBLIC";

    }

    public class RoleConstant
    {
        public const int ADMIN = 1;
        public const int USER = 2;
    }

    public class QuestionType
    {
        public const string TEXT = "TEXT";
        public const string UNTEXT = "UNTEXT";
    }

    public class Constant
    {
        public const int TotalCodeLength = 20;
        public const int ValidPasswordLength= 10;

    }

    public class ErrorMessage
    {
        //account
        public const string PASSWORD_NOT_VALID = "Password is not accepted";
        public const string ACCOUNT_ALREADY_EXIST = "This account already in the system";
        public const string ACCOUNT_IS_NOT_EXIST = "This account is not in the system";

        //exam
        public const string EXAM_NOT_FOUND = "Exam not found";
        public const string EXAM_ALREADY_TAKEN = "You have already taken this exam";
        public const string EXAM_NOT_PUBLIC = "Exam is not openned";

        //enroll code
        public const string CODE_NOT_VALID = "The code is not valid";

        //question
        public const string QUESTION_NOT_VALID_TO_CREATE = "Question is not valid to create";

        //account in stage
        public const string ACCOUNT_IN_STAGE_ALREADY_EXIST = "This account already exist";
    }
}
