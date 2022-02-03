using Xunit;
using BPOBackend;
using System;
using System.Collections.Generic;
using System.Text;

namespace BPOBackend.Tests
{
    public class AutomateLabelsBlTests
    {
        [Fact()]
        public void GetIdsByUserIdAndDateTest()
        {
            DateTime startDate = new DateTime(2022, 1, 1);
            DateTime endDate = new DateTime(2022, 1, 31);
            List<AutomateLabel> result = AutomateLabelsBl.Instance.GetIdsByUserIdAndDate("fe69add2-2149-44dc-9415-cdd640b36925", startDate, endDate);
            Assert.NotNull(result);
        }

        [Fact()]
        public void GetAutomateLabelTest()
        {
            DateTime startDate = new DateTime(2022, 1, 11);
            DateTime endDate = new DateTime(2022, 1, 31);
            List<AutomateLabel> result = AutomateLabelsBl.Instance.GetAutomateLabel("fe69add2-2149-44dc-9415-cdd640b36925", startDate, endDate);
            Assert.NotNull(result);
        }
    }
}