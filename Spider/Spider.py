import requests
import codecs

from bs4 import BeautifulSoup

class Spider:
    def __init__(self):
        self._url="https://www.pixiv.net/ranking.php?mode=daily"

    def _getBeautifulSoup(self):
        response = requests.get(self._url, timeout=5)
        if not response.status_code == 200:
            with open("output.tmp", "w") as fp:
                fp.write("Network Error")
            return None
        else:
            return BeautifulSoup(response.text, "lxml")

    def getImagePaths(self):
        bs = self._getBeautifulSoup()
        taskList = []
        if bs == None:
            return taskList
        else:
            items = bs.find_all("section")
            count = 0
            temp = ""
            for item in items:
                if item.has_attr("class"):
                    if item["class"][0] == "ranking-item":
                        taskList.append(item.img["data-src"])
                        temp += item["data-id"] + '#' + item["data-title"] + '#' + item["data-user-name"] + '#' + item.img["data-tags"] + '\n'
                        count += 1
                        if count == 10:
                            break
            with codecs.open("output.tmp", "w", "utf-8") as fp:
                fp.write(temp)
            return taskList
                        