using System;

namespace CommonModel
{
	public class SettingChangedEventArgs : EventArgs
	{

		public string Name { get; }
		public string NewValue { get; }

		public SettingChangedEventArgs(string name, string newValue)
		{
			Name = name;
			NewValue = newValue;
		}
	}

	public class GraphicEventArgs : EventArgs
	{
		public string Graphic { get; }

		public GraphicEventArgs(string graphic)
		{
			Graphic = graphic;
		}
	}

	public class GraphicsEventArgs : EventArgs
	{
		public string[] Graphics { get; }

		public GraphicsEventArgs(params string[] graphic)
		{
			Graphics = graphic;
		}
	}
}
