module.exports = {
  publicRuntimeConfig: {
    site: {
      name: "Freshaliens Leaderboard - VDP '22",
      url:
        process.env.NODE_ENV === "development"
          ? "http://localhost:3000"
          : "https://vdp22-freshaliens-leaderboard.vercel.app",
      title: "Freshaliens Leaderboard - VDP '22",
      description: "Freshaliens Leaderboard - VDP '22",
      socialPreview: "/images/preview.png",
    },
  },
  swcMinify: true,
  i18n: {
    locales: ["en-US"],
    defaultLocale: "en-US",
  },
};

module.exports = {
  async headers() {
    return [
      {
        // matching all API routes
        source: "/api/:path*",
        headers: [
          { key: "Access-Control-Allow-Credentials", value: "true" },
          { key: "Access-Control-Allow-Origin", value: "*" },
          {
            key: "Access-Control-Allow-Methods",
            value: "GET,OPTIONS,PATCH,DELETE,POST,PUT",
          },
          {
            key: "Access-Control-Allow-Headers",
            value:
              "X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Content-Type, Date, X-Api-Version",
          },
        ],
      },
    ];
  },
};
