## A* Algorithm

#### 车坐标模型

<center class="half">
<img src="img/car_cor.jpg" align="left" width="400" height="400" alt="model"/>   
   
</center>
当前模型旋转角度θ=π/2



#### 网格中车模型方向  

<center class="half">
<img src="img/grid_car.png" align="left" width="400"/>  
</center>  


#### 运动方向表格
|   θ    | Up    | Down   | Left  | Right |  
| :-----:|:-----:| :-----:|:-----:|:-----:|
|   π/2  |(-1,0) | (1,0)  | (0,-1)| (0,1) |
|   π    |(0,-1) | (0,1)  | (1,0) | (-1,0)|
|   0    | (0,1) | (0,-1) | (-1,0)| (1,0) |
|   -π/2 |(1,0)  | (-1,0) | (0,1) | (0,-1)|

#### 运动转化表格  
| Up    | Down   | Left  | Right |  
|:-----:| :-----:|:-----:|:-----:|
|(-sinθ,cosθ) | (sinθ,-cosθ)  | (-cosθ,-sinθ)| (cosθ,sinθ) |
