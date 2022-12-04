module.exports = {
  publicRuntimeConfig: {
    site: {
      name: "Freshaliens Leaderboard - VDP '22",
      url:
        process.env.NODE_ENV === "development"
          ? "http://localhost:3000"
          : "https://vdp22-freshaliens-leaderboard.vercel.app",
      title: "Next.js + Tailwind CSS template",
      description: "Next.js + Tailwind CSS template",
      socialPreview: "/images/preview.png",
    },
  },
  swcMinify: true,
  i18n: {
    locales: ["en-US"],
    defaultLocale: "en-US",
  },
};
