using Microsoft.AspNetCore.Identity;

namespace App.DAL.Localization
{
    public class AzerbaijaniIdentityErrorDescriber : IdentityErrorDescriber
    {
        // Password errors
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"?ifr? ?n az? {length} simvol olmal?d?r"
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "?ifr? ?n az? bir r?q?m (0-9) ehtiva etm?lidir"
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "?ifr? ?n az? bir kiçik h?rf (a-z) ehtiva etm?lidir"
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "?ifr? ?n az? bir böyük h?rf (A-Z) ehtiva etm?lidir"
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "?ifr? ?n az? bir xüsusi simvol (!@#$%^&* v? s.) ehtiva etm?lidir"
            };
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = $"?ifr? ?n az? {uniqueChars} f?rqli simvol ehtiva etm?lidir"
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = "?ifr?l?r uy?un g?lmir"
            };
        }

        // User errors
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = $"{email} art?q istifad? olunur"
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $"{userName} art?q istifad? olunur"
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = $"{email} etibars?z email ünvan?d?r"
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = $"{userName} etibars?zd?r. ?stifad?çi ad? yaln?z h?rf v? r?q?ml?rd?n ibar?t ola bil?r"
            };
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyHasPassword),
                Description = "?stifad?çinin art?q ?ifr?si mövcuddur"
            };
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyInRole),
                Description = $"?stifad?çi art?q '{role}' roluna malikdir"
            };
        }

        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserNotInRole),
                Description = $"?stifad?çi '{role}' roluna malik deyil"
            };
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError
            {
                Code = nameof(UserLockoutNotEnabled),
                Description = "Bu istifad?çi üçün bloklanma aktivl??dirilm?yib"
            };
        }

        // Role errors
        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = $"'{role}' rolu art?q mövcuddur"
            };
        }

        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(InvalidRoleName),
                Description = $"'{role}' etibars?z rol ad?d?r"
            };
        }

        // Token errors
        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = nameof(InvalidToken),
                Description = "Token etibars?zd?r"
            };
        }

        // Recovery code errors
        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError
            {
                Code = nameof(RecoveryCodeRedemptionFailed),
                Description = "B?rpa kodu etibars?zd?r"
            };
        }

        // Concurrency errors
        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description = "Optimistik yoxlama x?tas?, obyekt d?yi?dirilib"
            };
        }

        // Default error
        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = "X?ta ba? verdi"
            };
        }

        // Login errors
        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = "Bu login art?q ba?qa istifad?çiy? ba?l?d?r"
            };
        }
    }
}
