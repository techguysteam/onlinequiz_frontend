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
        public const string PENDING_STAGE = "PENDING";

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
        public const string EXAM_NOT_TAKEN = "You did not complete this exam";
        public const string EXAM_NOT_PUBLIC = "Exam is not openned";
        public const string EXAM_NOT_IN_PENDING = "Exam is already openned so you cannot edit";
        public const string EXAM_INVALID_TO_CREATE = "The exam is not valid to create";

        //enroll code
        public const string CODE_NOT_VALID = "The code is not valid";

        //question
        public const string QUESTION_NOT_VALID_TO_CREATE = "Question is not valid to create";
        public const string QUESTION_NOT_VALID = "Question is invalid";
        public const string QUESTION_NOT_FOUND = "Question is not existed";
        public const string QUESTION_NOT_ACTIVE = "Question is not active";
        public const string QUESTION_ALREADY_IN_EXAM = "Question is already in the exam";
        public const string QUESTION_NOT_IN_EXAM = "Question is not in the exam";
        public const string QUESTION_CANNOT_DELETE_PERMANENTLY = "Cannot delete it, it may have effect to other features";

        //account in stage
        public const string ACCOUNT_IN_STAGE_ALREADY_EXIST = "This account already exist";

        //stage
        public const string STAGE_NOT_PUBLIC = "This stage is not public";
        public const string STAGE_NOT_EXIST = "This stage is not exist";
    }
}

