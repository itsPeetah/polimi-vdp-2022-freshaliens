import internal from "stream";

export type Score = {
  level: number;
  time: string;
};

export type LeaderboardEntry = {
  name: string;
  score: Score;
};

export type Leaderboard = {
  entries: LeaderboardEntry[];
};
