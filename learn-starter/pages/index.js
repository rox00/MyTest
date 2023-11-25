
// import MyColor from '../components/mycolor'
import { useState } from 'react';
import myteststyle from '../styles/mytest.module.css';

export default function Home({ allPostsData1, allPostsData, getAllPostIdsvar }) {
    var [grpcdata, setgrpcdata] = useState('123')
    var [color, setColor] = useState("#9966ff");
    var timer = setTimeout(async () => {
        // color == "#9966ff" ? setColor("orange") : setColor("#9966ff");
        let result = await fetch('http://localhost:9000').then(res => res.text());//.then(res => res.text())
        setgrpcdata(result)
        setColor(result)
    }, 3000)


    return <>
        {/* <MyColor color="#9966ff"></MyColor> */}
        <table>
            <tbody>
                <tr>
                    <td>NGBT Status: {grpcdata}</td>
                    <td style={{ background: color, verticalAlign: 'middle', height: '1rem', width: '4rem' }}></td>
                </tr>
                <tr>
                    <td>KIOSK Status:</td>
                    <td style={{ background: 'red', marginTop: '5px', verticalAlign: 'middle', height: '1rem', width: '4rem' }}></td>
                </tr>
                <tr>
                    <td>NTBC Status:</td>
                    <td style={{ background: 'green', marginTop: '5px', verticalAlign: 'middle', height: '1rem', width: '4rem' }}></td>
                </tr>
                <tr>
                    <td>CBGW Status:</td>
                    <td style={{ background: 'blue', marginTop: '5px', verticalAlign: 'middle', height: '1rem', width: '4rem' }}></td>
                </tr>
            </tbody>
        </table>
        <style jsx>{`
          .div{
            color:red
          }
          .aa{
              color:blue
          }
          .cs{
              color:${color}
          }
          .text,button{
            color:${color}
        }
        `}</style>
    </>
}