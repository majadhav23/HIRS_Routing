package TestAPI;
import org.openqa.selenium.chrome.ChromeOptions;
import org.openqa.selenium.remote.CapabilityType;
import java.net.MalformedURLException;
import java.net.URL;

import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.firefox.FirefoxOptions;
import org.openqa.selenium.remote.RemoteWebDriver;
import org.testng.annotations.Test;

public class NewTest {
	
	WebDriver driver;
	@Test
	public void test1() {
		//driver = new ChromeDriver();
		ChromeOptions capability = new ChromeOptions();
		capability.setCapability(CapabilityType.ACCEPT_SSL_CERTS, true);
		capability.setCapability(CapabilityType.ACCEPT_INSECURE_CERTS,true);

		WebDriver driver = new ChromeDriver(capability);
		driver.get("https://127.0.0.1:7136/todo/edit/8");
		String title = driver.getTitle();
		System.out.println(title);
		driver.quit();
	}
	
}