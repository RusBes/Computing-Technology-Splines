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

    public enum UnevenTypes
    {
        Normal,
        Star,
        Rectangle,
        Petla
    }

    public class UnEvenTypesEventArgs : EventArgs
    {
        public UnevenTypes Type { get; }
        public string Graphic { get; }

        public UnEvenTypesEventArgs(string graphic, UnevenTypes type)
        {
            Graphic = graphic;
            Type = type;
        }
    }

    public class EntityEventArgs : EventArgs
	{
		public string Name { get; }

		public EntityEventArgs(string name)
		{
			Name = name;
		}
	}


    
        public class PicureBoxFilterEventArgs : EventArgs
    {
        public string Filter { get; }

        public PicureBoxFilterEventArgs(string filter)
        {
            Filter = filter;
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

    public enum ScopeStates
    {
        Off,
        On,
        Applied
    }
}
