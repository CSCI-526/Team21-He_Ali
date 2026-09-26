# Redline Rush

- [Play on Unity Play](https://play.unity.com/api/v1/games/game/89f21a1c-9f23-48b2-a5fe-1f46184a3b38/build/latest/frame)
- [Descriptive document](https://docs.google.com/document/d/158VhF6BgA7K4MI-C9BUlZdGnkoFmhDPkxYktXaQlbyE/edit?usp=sharing)
- [Gameplay video](https://youtu.be/1QeiQiOD2BY)

## Deploy to GitHub Pages

1. In Unity, open **File > Build Profiles**, select **Web**, and switch to that platform.
2. Open **Edit > Project Settings > Player > Web > Publishing Settings** and keep **Decompression Fallback** enabled.
3. Build the Web version into the repository's `docs` folder.
4. Make sure `docs/.nojekyll` still exists after the build.
5. Commit the updated project and `docs` files, then push `main` to GitHub.
6. In the GitHub repository, open **Settings > Pages**. Under **Build and deployment**, choose **Deploy from a branch**, then select `main` and `/docs` and click **Save**.

The site will be available at <https://tianyuhe11.github.io/PairProject/> after GitHub finishes deploying it.
