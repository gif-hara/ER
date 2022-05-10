namespace HK.Framework.EventSystems
{
    /// <summary>
    /// <see cref="Broker"/>を介して通知されるイベント
    /// </summary>
    public abstract class Message
    {
    }

    public abstract class Message<E> : Message
        where E : Message<E>, new()
    {
        protected static E cache = new E();
        
        /// <summary>
        /// イベントを取得します
        /// </summary>
        public static E Get()
        {
            return cache;
        }
    }

    public abstract class Message<E, P1> : Message
        where E : Message<E, P1>, new()
    {
        protected P1 param1;
        
        protected static E cache = new E();

        /// <summary>
        /// イベントを取得します
        /// </summary>
        public static E Get(P1 param1)
        {
            cache.param1 = param1;

            return cache;
        }

        public void Deconstruct(out P1 param1)
        {
            param1 = this.param1;
        }
    }

    public abstract class Message<E, P1, P2> : Message
        where E : Message<E, P1, P2>, new()
    {
        protected P1 param1;

        protected P2 param2;
        
        protected static E cache = new E();

        /// <summary>
        /// イベントを取得します
        /// </summary>
        public static E Get(P1 param1, P2 param2)
        {
            cache.param1 = param1;
            cache.param2 = param2;

            return cache;
        }

        public void Deconstruct(out P1 param1, out P2 param2)
        {
            param1 = this.param1;
            param2 = this.param2;
        }
    }

    public abstract class Message<E, P1, P2, P3> : Message
        where E : Message<E, P1, P2, P3>, new()
    {
        protected P1 param1;

        protected P2 param2;

        protected P3 param3;
        
        protected static E cache = new E();

        /// <summary>
        /// イベントを取得します
        /// </summary>
        public static E Get(P1 param1, P2 param2, P3 param3)
        {
            cache.param1 = param1;
            cache.param2 = param2;
            cache.param3 = param3;

            return cache;
        }

        public void Deconstruct(out P1 param1, out P2 param2, out P3 param3)
        {
            param1 = this.param1;
            param2 = this.param2;
            param3 = this.param3;
        }
    }

    public abstract class Message<E, P1, P2, P3, P4> : Message
        where E : Message<E, P1, P2, P3, P4>, new()
    {
        protected P1 param1;

        protected P2 param2;

        protected P3 param3;

        protected P4 param4;
        
        protected static E cache = new E();

        /// <summary>
        /// イベントを取得します
        /// </summary>
        public static E Get(P1 param1, P2 param2, P3 param3, P4 param4)
        {
            cache.param1 = param1;
            cache.param2 = param2;
            cache.param3 = param3;
            cache.param4 = param4;

            return cache;
        }

        public void Deconstruct(out P1 param1, out P2 param2, out P3 param3, out P4 param4)
        {
            param1 = this.param1;
            param2 = this.param2;
            param3 = this.param3;
            param4 = this.param4;
        }
    }
}
