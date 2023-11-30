
// import MyColor from '../components/mycolor'
import Head from 'next/head'
import { useState } from 'react';
import myteststyle from '../styles/mytest.module.css';
import Mylayout from '../components/mylayout.js';

export default function Home({ allPostsData1, allPostsData, getAllPostIdsvar }) {
    // var [grpcdata, setgrpcdata] = useState('123')
    var [color, setColor] = useState("#9966ff");
    const [position, setPosition] = useState({ x: 0, y: 0 });
    // var timer = setTimeout(async () => {
    //     // color == "#9966ff" ? setColor("orange") : setColor("#9966ff");
    //     var result = await fetch('http://localhost:9000').then(res => res.text())

    //     console.log(timer)
    //     // setgrpcdata(result)
    //     setColor(result)
    // }, 2000)


    return <>
        {/* <div style={{width:'10rem', Height:'10rem', display:'flex', justifyContent:'center', alignItems:'center', background:'red'}}>
        123456
    </div> */}
            <Mylayout></Mylayout>

        <div style={{ minHeight: '100vh', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
            {/* <MyColor color="#9966ff"></MyColor> */}
            {/* <div onPointerMove={e => {setPosition({x: e.clientX,y: e.clientY});}} style={{position: 'relative', width: '100vw',height: '100vh',}}>
      <div style={{position: 'absolute',backgroundColor: 'red', borderRadius: '50%',transform: `translate(${position.x}px, ${position.y}px)`,
        left: -10,
        top: -10,
        width: 20,
        height: 20,
      }} />
    </div> */}
            <table>
                <tbody>
                    <tr>
                        <td>NGBT Status: {color.toString()}</td>
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
        </div>
    </>
}