using System;

namespace HospitalManagementSystem.Models
{
    // Custom exception: thrown when an appointment would clash with one the doctor already has.
    public class DoctorUnavailableException : Exception
    {
        public DoctorUnavailableException(string message) : base(message) { }
    }

    // Custom exception: thrown when a record with the same unique key already exists.
    public class DuplicateRecordException : Exception
    {
        public DuplicateRecordException(string message) : base(message) { }
    }

    // Custom exception: thrown when a lookup by id/username finds nothing.
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException(string message) : base(message) { }
    }

    // Custom exception: thrown by LoginForm when credentials don't match.
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string message) : base(message) { }
    }
}
