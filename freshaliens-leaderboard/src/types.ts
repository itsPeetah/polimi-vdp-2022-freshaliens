import internal from "stream";

export type Score = {
  level: string;
  time: string;
};

export type LeaderboardEntry = {
  name: string;
  times: string[];
};

export type Leaderboard = {
  entries: LeaderboardEntry[];
};
