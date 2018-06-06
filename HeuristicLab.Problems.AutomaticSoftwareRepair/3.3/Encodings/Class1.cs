
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;


//public class TestClass {

//[Test]
//public void test1 ()  {  
//		var count = Syllables.Count("a"); 
//		Assert.That(count, Is.EqualTo(1));
//    }
//    [Test]
// public void test10 ()  {
//        var count = Syllables.Count ("snow white 123 ><");
//        Assert.That(count, Is.EqualTo(3));
//    }
//    [Test]
// public void test2 ()  {
//       var count = Syllables.Count("i o");
//        Assert.That(count, Is.EqualTo(2));
//    }
//    [Test]
// public void test3 ()  {
//       var count = Syllables.Count("mnhd");
//		Assert.That(count, Is.EqualTo(0));
//    }
//    [Test]
// public void test4 ()  {
//       var count = Syllables.Count("hello world");
//        Assert.That(count, Is.EqualTo(3));
//    }
//    [Test]
// public void test5 ()  {
//        var count = Syllables.Count("aeiou");
//		Assert.That(count, Is.EqualTo(5));
//    }
//    [Test]
// public void test6 ()  {
//      var count = Syllables.Count("seasons greetings!");
//		Assert.That(count, Is.EqualTo(6));
//    }
//    [Test]
// public void test7 ()  {
//       var count = Syllables.Count ("which witch is which?");
//        Assert.That(count, Is.EqualTo(4));
//    }
//    [Test]
// public void test8 ()  {
//       var count = Syllables.Count("!@#$%^,");
//        Assert.That(count, Is.EqualTo(0));
//    }
//    [Test]
// public void test9 ()  {
//        var count = Syllables.Count ("123zdh");
//        Assert.That(count, Is.EqualTo(0));
//    }
	
//	 [Test]
// public void test11 ()  {
//        var count = Syllables.Count ("khd");
//        Assert.That(count, Is.EqualTo(0));
//    }
//    [Test]
// public void test12 ()  {
//        var count = Syllables.Count ("aeiouy");
//        Assert.That(count, Is.EqualTo(6));
//    }
//    [Test]
// public void test13 ()  {
//		 var count = Syllables.Count ("here and there");	
//        Assert.That(count, Is.EqualTo(5));
//    }
//    [Test]
// public void test14 ()  {
//        var count = Syllables.Count ("bbbbbbb a");
//        Assert.That(count, Is.EqualTo(1));
//    }
//    [Test]
// public void test15 ()  {
//       var count = Syllables.Count("9876543210");
//       Assert.That(count, Is.EqualTo(0));
//    }
//    [Test]
// public void test16 ()  {
//        var count = Syllables.Count ("1 a 2 e 3 $#@ u");
//        Assert.That(count, Is.EqualTo(3));
//    }	
//}

[TestFixture, Timeout (1000), SingleThreaded, NonParallelizable]
public class BucketSortTest {
  [Test]
  public void Test_0 () {
    //java.util.ArrayList result = java_programs.BUCKETSORT.bucketsort(new java.util.ArrayList(java.util.Arrays.asList(3,11,2,9,1,5)),(int)12);
    //String resultFormatted = java_testcases.junit.QuixFixOracleHelper.format(result,true);
    //org.junit.Assert.assertEquals("[1,2,3,5,9,11]", resultFormatted);
    var result = Bucketsort.Sort (new List<int> { 3, 11, 2, 9, 1, 5 }, 12);
    Assert.That (result, Is.EqualTo (new List<int> { 1, 2, 3, 5, 9, 11 }));
  }

  [Test]
  public void Test_1 () {
    //java.util.ArrayList result = BUCKETSORT.bucketsort(new java.util.ArrayList(java.util.Arrays.asList(3,2,4,2,3,5)),(int)6);
    //String resultFormatted = java_testcases.junit.QuixFixOracleHelper.format(result,true);
    //org.junit.Assert.assertEquals("[2,2,3,3,4,5]", resultFormatted);

    var result = Bucketsort.Sort (new List<int> { 3, 2, 4, 2, 3, 5 }, 6);
    Assert.That (result, Is.EqualTo (new List<int> { 2, 2, 3, 3, 4, 5 }));
  }

  [Test]
  public void Test_2 () {
    //java.util.ArrayList result = BUCKETSORT.bucketsort(new java.util.ArrayList(java.util.Arrays.asList(1,3,4,6,4,2,9,1,2,9)),(int)10);
    //String resultFormatted = java_testcases.junit.QuixFixOracleHelper.format(result,true);
    //org.junit.Assert.assertEquals("[1,1,2,2,3,4,4,6,9,9]", resultFormatted);

    var result = Bucketsort.Sort (new List<int> { 1, 3, 4, 6, 4, 2, 9, 1, 2, 9 }, 10);
    Assert.That (result, Is.EqualTo (new List<int> { 1, 1, 2, 2, 3, 4, 4, 6, 9, 9 }));

  }

  [Test]
  public void Test_3 () {
    //java.util.ArrayList result = BUCKETSORT.bucketsort(new java.util.ArrayList(java.util.Arrays.asList(20,19,18,17,16,15,14,13,12,11)),(int)21);
    //String resultFormatted = java_testcases.junit.QuixFixOracleHelper.format(result,true);
    //org.junit.Assert.assertEquals("[11,12,13,14,15,16,17,18,19,20]", resultFormatted);

    var result = Bucketsort.Sort (new List<int> { 20, 19, 18, 17, 16, 15, 14, 13, 12, 11 }, 21);
    Assert.That (result, Is.EqualTo (new List<int> { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }));
  }

  [Test]
  public void Test_4 () {
    //java.util.ArrayList result = BUCKETSORT.bucketsort(new java.util.ArrayList(java.util.Arrays.asList(20,21,22,23,24,25,26,27,28,29)),(int)30);
    //String resultFormatted = java_testcases.junit.QuixFixOracleHelper.format(result,true);
    //org.junit.Assert.assertEquals("[20,21,22,23,24,25,26,27,28,29]", resultFormatted);

    var result = Bucketsort.Sort (new List<int> { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 }, 30);
    Assert.That (result, Is.EqualTo (new List<int> { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 }));
  }

  [Test]
  public void Test_5 () {
    //java.util.ArrayList result = BUCKETSORT.bucketsort(new java.util.ArrayList(java.util.Arrays.asList(8,5,3,1,9,6,0,7,4,2,5)),(int)10);
    //String resultFormatted = java_testcases.junit.QuixFixOracleHelper.format(result,true);
    //org.junit.Assert.assertEquals("[0,1,2,3,4,5,5,6,7,8,9]", resultFormatted);

    var result = Bucketsort.Sort (new List<int> { 8, 5, 3, 1, 9, 6, 0, 7, 4, 2, 5 }, 10);
    Assert.That (result, Is.EqualTo (new List<int> { 20, 1, 2, 3, 4, 5, 5, 6, 7, 8, 9 }));
  }
}

/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

  /**
   *
   * @author derricklin
   */
  public class Bucketsort {
    public static List<int> Sort (List<int> arr, int k) {
      var counts = new List<int> (Enumerable.Range (0, k).Select (x => 0));
      foreach (int x in arr) {
        counts[x] = counts[x] + 1;
      }

      List<int> sorted_arr = new List<int> (100);
      int i = 0;
      foreach (int count in arr) {
        // arr is counts in fixed version
        sorted_arr.AddRange (Enumerable.Range (0, count).Select (x => i));
        i++;
      }

      return sorted_arr;
    }
  }
