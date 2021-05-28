﻿//--------------------------------------------------------------------------------------------------
// Copyright © Nezaboodka™ Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0.
//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Nezaboodka.Nevod
{
    internal sealed class HavingCandidate : CompoundCandidate
    {
        private List<RejectionTargetCandidate> fRelatedTargetCopies;

        public HavingCandidate(HavingExpression expression)
            : base(expression)
        {
        }

        public override void OnElementMatch(Candidate element, MatchingEvent matchingEvent)
        {
            matchingEvent.UpdateEventObserver(this);
            End = matchingEvent.Location;
            SearchContext.AddToPendingHavingCandidates(this);
            CompleteMatch(matchingEvent);
        }

        public void AddTargetCandidate(RejectionTargetCandidate target)
        {
            if (fRelatedTargetCopies == null)
                fRelatedTargetCopies = new List<RejectionTargetCandidate>();
            fRelatedTargetCopies.Add(target);
        }

        public void RemoveTargetCandidate(RejectionTargetCandidate target)
        {
            if (fRelatedTargetCopies != null)
                fRelatedTargetCopies.Remove(target);
        }

        internal void OnInnerPatternMatch()
        {
            RejectionTargetCandidate target = GetRejectionTargetCandidate();
            if (!target.IsRejected)
                target.RemovePendingHavingCandidate(this);
            if (fRelatedTargetCopies != null)
            {
                for (int i = fRelatedTargetCopies.Count - 1; i >= 0; i--)
                {
                    target = fRelatedTargetCopies[i];
                    if (!target.IsRejected)
                        target.RemovePendingHavingCandidate(this);
                }
            }
        }

        internal void OnInnerPatternReject()
        {
            RejectTarget();
            if (fRelatedTargetCopies != null)
            {
                for (int i = fRelatedTargetCopies.Count - 1; i >= 0; i--)
                    fRelatedTargetCopies[i].Reject();
            }
        }
    }
}
