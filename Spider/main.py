from Spider import *
from Download import *

if __name__ == "__main__":
    spider = Spider()
    manager = DownloadManager(spider.getImagePaths())