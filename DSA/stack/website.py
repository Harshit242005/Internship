class Website:
    def __init__(self, website):
        self.website = website
        self.forward = self.back = None

class Browser:
    def __init__(self):
        self.count = 0
        self.homepage = self.lastpage = None
        self.back = None

    def forward_website(self, url):
        new_website = Website(url)
        if self.homepage is None:
            self.homepage = self.lastpage = new_website
            self.count += 1
        else:
            self.lastpage.forward = new_website
            new_website.forward = None
            new_website.back = self.lastpage
            self.lastpage = new_website

    def back_website(self, step):
        if self.homepage is None:
            return None
        if step > self.count:
            return self.homepage.website
        temp = self.lastpage
        i = self.count
        while i > step and temp is not None:
            temp = temp.back
            i -= 1
        return temp.website if temp else None
    
    def total_website_searched(self):
        if self.homepage is None:
            return 0
        count_website = 0
        temp = self.homepage
        while temp is not None:
            count_website += 1
            temp = temp.forward
        return f"Total websites searched by the user: {count_website}"
    

if __name__ == "__main__":
    browser = Browser()
    browser.forward_website("www.google.com")
    browser.forward_website("www.gfg.com")  
    browser.forward_website("www.w3schools.com")
    print(browser.back_website(7))
    print(browser.total_website_searched())
