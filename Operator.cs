namespace CalcClasses
{
    public struct Operator
    {
        // Элементы дынных - публичные для простоты
        /// <summary>
        /// Какая операция: *,/,+,-,...
        /// </summary>
        public string type;
        /// <summary>
        /// Приоритет операции 0,1..   0- самый высокий
        /// </summary>
        public int prior;
        public int firstPos;
        public int lastPos;
    }
}
