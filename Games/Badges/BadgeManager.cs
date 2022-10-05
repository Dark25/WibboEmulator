﻿using WibboEmulator.Database.Interfaces;

namespace WibboEmulator.Games.Badges
{
    public class BadgeManager
    {
        private List<string> _notAllowed;

        public BadgeManager()
        {
            this._notAllowed = new List<string>();
        }

        public void Init(IQueryAdapter dbClient)
        {
            this._notAllowed.Clear();

            string badgeNotAllowed = WibboEnvironment.GetSettings().GetData<string>("badge.not.allowed");

            this._notAllowed.AddRange(badgeNotAllowed.Split(','));
        }

        public bool HaveNotAllowed(string badgeId)
        {
            if (this._notAllowed.Contains(badgeId))
                return true;

            if (badgeId.StartsWith("MRUN"))
                return true;

            return false;
        }

        public List<string> GetNotAllowed() => this._notAllowed;
    }
}
