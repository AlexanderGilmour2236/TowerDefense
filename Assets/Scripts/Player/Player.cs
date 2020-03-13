using Misc;

namespace Player
{
    public class Player
    {
        public ReactiveProperty<int> Health = new ReactiveProperty<int>();
        public ReactiveProperty<float> Gold = new ReactiveProperty<float>();
        public ReactiveProperty<int> CurrentWaveCount = new ReactiveProperty<int>();
    }
}