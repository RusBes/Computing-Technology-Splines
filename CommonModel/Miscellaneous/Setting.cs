using System;

namespace CommonModel.Miscellaneous
{
	public class Setting
	{
		public string Name { get; }
		public string LabelText { get; }
		private object _value;
		public object Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				SettingChanged?.Invoke(this, new SettingChangedEventArgs(this.Name, _value.ToString()));
			}
		}
		public bool IsForShow { get; set; }
		public EventHandler<SettingChangedEventArgs> SettingChanged;

		public Setting(string name, string labelText, object value, bool isForShow = false)
		{
			Name = name;
			LabelText = labelText;
			Value = value;
			IsForShow = isForShow;
		}
	}
}
