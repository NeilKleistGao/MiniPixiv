import requests
class DownloadManager:
    def __init__(self, taskList):
        self._taskList = taskList
        self._header = { "Referer": "https://accounts.pixiv.net/login?lang=zh&source=pc&view_type=page&ref=wwwtop_accounts_index",
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; WOW64) "
            "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36" }
        self._download()
    def _download(self):
        count = 1
        for item in self._taskList:
            response = requests.get(item, headers = self._header)
            if not response.status_code == 200:
                continue
            with open(str(count) + ".jpg", "wb") as fp:
                fp.write(response.content)
            count += 1