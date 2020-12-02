using AdventOfCode2020;
using AdventOfCode2020.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using LX = System.Linq.Expressions;

namespace AdventOfCode2020.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ChallengeInfo SelectedChallenge
        {
            get => (ChallengeInfo)GetValue(selectedChallengeDp);
            set
            {
                Part1.TestResults = Part2.TestResults = TestResults.None;
                SetValue(selectedChallengeDp, value);

                var instance = value.Create();

                Part1.TestResults = GetTestResult(() => instance.Part1Test());
                Part2.TestResults = GetTestResult(() => instance.Part2Test());
            }
        }

        private static TestResults GetTestResult(Action runTest)
        {
            try
            {
                runTest();
                return TestResults.Passed;
            }
            catch (NotImplementedException)
            {
                return TestResults.NotImplemented;
            }
            catch
            {
                return TestResults.NotImplemented;
            }
        }

        public IReadOnlyList<ChallengeInfo> Challenges { get; }

        public string Input { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Challenges = typeof(ChallengeBase).Assembly.GetTypes()
                .Where(x => typeof(ChallengeBase).IsAssignableFrom(x))
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsDefined(typeof(ChallengeAttribute), false))
                .Select(ChallengeInfo.FromType)
                .ToList();

            SelectedChallenge = Challenges.Last();
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedChallenge == null)
                return;

            var instance = SelectedChallenge.Create();
            var pt1Result = Measure(() => instance.Part1(Input), out var pt1Metrics);
            var pt2Result = Measure(() => instance.Part2(Input), out var pt2Metrics);

            Part1.Update(pt1Result, pt1Metrics);
            Part2.Update(pt2Result, pt2Metrics);
        }

        private object Measure(Func<object> action, out Metrics metrics)
        {
            try
            {
                object result = null;
                metrics = Metrics.Measure(maxMilliseconds: 150, minReps: 10, () =>
                {
                    var newResult = action();
                    if (result != null && !result.Equals(newResult))
                        throw new Exception($"Result differed between test runs! Old: {result}, New: {newResult}");

                    result = newResult;
                });

                return result;
            }
            catch (NotImplementedException)
            {
                metrics = Metrics.Empty;
                return "Not yet implemented";
            }
            catch { throw; }
        }

        public class ChallengeInfo
        {
            public string Name { get; }

            public Factory Create { get; }

            public static ChallengeInfo FromType(Type type)
            {
                var attr = type.GetCustomAttribute<ChallengeAttribute>(false);
                var name = $"Day {attr.DayNum} - {attr.Title}";
                var factory = CreateFactory(type);
                return new ChallengeInfo(name, factory);
            }

            private ChallengeInfo(string name, Factory func)
            {
                Name = name;
                Create = func;
            }

            private static Factory CreateFactory(Type challengeType)
            {
                var ctor = challengeType.GetConstructor(Type.EmptyTypes);
                var fac = LX.Expression.New(ctor);
                return LX.Expression.Lambda<Factory>(fac).Compile();
            }

            public delegate ChallengeBase Factory();
        }

        private static readonly DependencyProperty selectedChallengeDp =
            DependencyProperty.Register("SelectedChallenge",
            typeof(ChallengeInfo), typeof(MainWindow),
            new PropertyMetadata(null, (s, e) => ((MainWindow)s).SelectedChallenge = (ChallengeInfo) e.NewValue));
    }
}
