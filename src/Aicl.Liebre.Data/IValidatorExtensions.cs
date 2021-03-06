﻿using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Internal;
using ServiceStack.FluentValidation.Results;
using ServiceStack;

namespace Aicl.Liebre.Data
{
	public static class IValidatorExtensions
	{
		public static void ValidateAndThrowHttpError<T>(this IValidator<T> validator, T instance) {
			var result = validator.Validate(instance);


			if(! result.IsValid)
			{	
				throw new HttpError( result.BuildErrorMessage());
			}

		}

		public static void ValidateAndThrowHttpError<T>(this IValidator<T> validator, T instance, string ruleSet)
		{
			var result = validator.Validate(instance, (IValidatorSelector)null, ruleSet);

			if (!result.IsValid)
			{
				throw new HttpError( result.BuildErrorMessage());
			}
		}


		public static string BuildErrorMessage(this ValidationResult validationResult)
		{
			return validationResult.Errors.SerializeToString();	
		}

		public static void MyValidate<T>(this IValidator<T> validator, T instance, params string[] rules){
			var selector = new RulesetValidatorSelector (rules);

			var context = new ValidationContext<T> (instance, new PropertyChain (), selector);
			var validationResult= validator.Validate (context);
			if (!validationResult.IsValid) {
				throw new ValidationException (validationResult.Errors);
			}
		}

	}
}

