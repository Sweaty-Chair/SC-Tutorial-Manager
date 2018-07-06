using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SweatyChair {

	public static class InputFieldUtility {

		public static void SetValidationData(this InputField field,InputFieldValidationData validationData)
		{
			field.characterLimit = validationData.characterLimit;
			field.characterValidation = validationData.characterValidation;
			field.inputType = validationData.inputType;
			field.keyboardType = validationData.keyboardType;
		}

	}


	public class InputFieldValidationData {

		public int characterLimit;
		public InputField.CharacterValidation characterValidation;
		public InputField.InputType inputType;
		public TouchScreenKeyboardType keyboardType;

		public InputFieldValidationData()
		{
			characterLimit = 0;
			characterValidation = InputField.CharacterValidation.None;
			inputType = InputField.InputType.Standard;
			keyboardType = TouchScreenKeyboardType.Default;
		}

		public InputFieldValidationData(int characterLimit, InputField.CharacterValidation characterValidation, InputField.InputType inputType, TouchScreenKeyboardType keyboardType)
		{
			this.characterLimit = characterLimit;
			this.characterValidation = characterValidation;
			this.inputType = inputType;
			this.keyboardType = keyboardType;
		}

	}

}